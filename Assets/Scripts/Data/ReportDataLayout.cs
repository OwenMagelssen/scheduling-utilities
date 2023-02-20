using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace SchedulingUtilities
{
	// public enum ReportDataType
	// {
	// 	Text,
	// 	Shift,
	// 	EmployeeName,
	// 	JobTitle,
	// 	Date,
	// 	Time,
	// 	ShiftLength,
	// 	DateTime,
	// 	Status
	// }

	public enum JobTitle
	{
		SecurityOfficer,
		SecuritySupervisor,
		Other
	}

	public enum Status
	{
		Denied,
		Approved,
		Pending
	}

	public abstract class EtmReportItemProcessor
	{
		public abstract T CreateReport<T>(List<string> values) where T : EtmReportItem;
	}

	public class EtmTimeOffRequestProcessor : EtmReportItemProcessor
	{
		public override T CreateReport<T>(List<string> values)
		{
			var timeOffRequest = new EtmTimeOffRequest();
			timeOffRequest.EmployeeName = values[2];
			timeOffRequest.JobTitle = values[3] switch
			{
				"Security Officer" => JobTitle.SecurityOfficer,
				"Security Working Supervisor" => JobTitle.SecuritySupervisor,
				_ => JobTitle.Other
			};
			CultureInfo culture = new CultureInfo("en-US");   
			timeOffRequest.TimeOffStart = DateTime.Parse($"{values[4]} {values[5]}", culture);
			float.TryParse(values[6], out float dayHours);
			float.TryParse(values[7], out float nightHours);
			timeOffRequest.Hours = dayHours + nightHours;
			timeOffRequest.RequestedOn = DateTime.Parse(values[8], culture);
			timeOffRequest.Status = Status.Pending;
			return timeOffRequest as T;
		}
	}

	public abstract class EtmReportItem { }

	[Serializable]
	public class EtmTimeOffRequest : EtmReportItem
	{
		public string EmployeeName;
		public JobTitle JobTitle;
		public DateTime TimeOffStart;
		public float Hours;
		public DateTime RequestedOn;
		public Status Status;
	}
	
	public interface Report
	{
		public EtmReportItemProcessor ItemProcessor { get; set; }
		public bool CsvHasHeader { get; set; }

		public List<T> ProcessEtmReport<T>(string csvFilePath) where T : EtmReportItem
		{
			var items = new List<T>();
			
			var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
			{
				HasHeaderRecord = false
			};

			using var streamReader = File.OpenText(csvFilePath);
			using var csvReader = new CsvReader(streamReader, csvConfig);
			var row = new List<string>();

			while (csvReader.Read())
			{
				row.Clear();
                
				for (int i = 0; csvReader.TryGetField(i, out string value); i++)
				{
					row.Add(value);
				}

				items.Add(ItemProcessor.CreateReport<T>(row));
			}

			return items;
		}
	}
}