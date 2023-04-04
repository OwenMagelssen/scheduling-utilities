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

		public void SortByName()
		{
			if (timeOffRequests == null) return;
			timeOffRequests.Sort(_nameSorter);
		}
		
		public void SortByNameReverse()
		{
			if (timeOffRequests == null) return;
			timeOffRequests.Sort(_nameReverseSorter);
		}
	}
}