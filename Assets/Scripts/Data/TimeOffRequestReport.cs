using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace SchedulingUtilities
{
	[ExecuteAlways, CreateAssetMenu(fileName = "New Time Off Request Report", menuName = "Scheduling Utilities/Time Off Request Report", order = 49)]
	public class TimeOffRequestReport : ScriptableObject, IReport
	{
		public List<TimeOffRequest> timeOffRequests;
		public string etmReportCsvFilePath;
		public EtmReportItemProcessor ItemProcessor => _timeOffRequestProcessor;
		private readonly EtmTimeOffRequestProcessor _timeOffRequestProcessor = new ();
		public bool CsvHasHeader => true;

		public string EtmReportFileName => Path.GetFileNameWithoutExtension(etmReportCsvFilePath);

		private TimeOffRequest.SortName _nameSorter = new();
		private TimeOffRequest.SortNameReverse _nameReverseSorter = new();
		private TimeOffRequest.SortTitle _titleSorter = new();
		private TimeOffRequest.SortTitleReverse _titleReverseSorter = new ();

		public void CreateReport()
		{
			var report = this as IReport;
			timeOffRequests = report.ProcessEtmReport<TimeOffRequest>(etmReportCsvFilePath);
		}

		public string AsJson()
		{
			var settings = new JsonSerializerSettings();
			settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			return JsonConvert.SerializeObject(timeOffRequests, settings);
		}

		public void SortByName() => timeOffRequests?.Sort(_nameSorter);
		
		public void SortByNameReverse() => timeOffRequests?.Sort(_nameReverseSorter);
		
		public void SortByTitle() => timeOffRequests?.Sort(_titleSorter);
		
		public void SortByTitleReverse() => timeOffRequests?.Sort(_titleReverseSorter);
	}
}