using System;
using System.Collections.Generic;

namespace SchedulingUtilities
{
	public enum ReportDataType
	{
		Text,
		Shift,
		EmployeeName,
		JobTitle,
		Date,
		Time,
		ShiftLength,
		DateTime,
		Status
	}

	public enum JobTitle
	{
		SecurityOfficer
	}

	public enum Status
	{
		Denied,
		Approved
	}
	
	[Serializable]
	public class EtmReportColumnDefinition
	{
		public string CsvValue;
		public bool Ignore;
		public string Label;
		public ReportDataType DataType;
	}

	[Serializable]
	public class EtmTimeOffRequest
	{
		public string EmployeeName;
		public JobTitle JobTitle;
		public DateTime TimeOffStart;
		public float Hours;
		public DateTime RequestedOn;
		public Status Status;
	}
	
	public abstract class  ReportDataLayout
	{
		public List<EtmReportColumnDefinition> Header;
		public abstract bool CsvHasHeader { get; set; }
	}
}