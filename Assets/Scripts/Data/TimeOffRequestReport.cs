using System;
using System.Collections;
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
		private readonly EtmTimeOffRequestProcessor _timeOffRequestProcessor = new ();
		public EtmReportItemProcessor ItemProcessor => _timeOffRequestProcessor;
		public bool CsvHasHeader => true;

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
	}
}