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

		private readonly TimeOffRequest.SortName _nameSorter = new();
		private readonly TimeOffRequest.SortNameReverse _nameReverseSorter = new();
		private readonly TimeOffRequest.SortTitle _titleSorter = new();
		private readonly TimeOffRequest.SortTitleReverse _titleReverseSorter = new();
		private readonly TimeOffRequest.SortTimeOffStart _timeOffStartSorter = new();
		private readonly TimeOffRequest.SortTimeOffStartReverse _timeOffStartReverseSorter = new();
		private readonly TimeOffRequest.SortHours _hoursSorter = new();
		private readonly TimeOffRequest.SortHoursReverse _hoursReverseSorter = new();
		private readonly TimeOffRequest.SortDateTimeRequested _dateTimeRequestedSorter = new();
		private readonly TimeOffRequest.SortDateTimeRequestedReverse _dateTimeRequestedReverseSorter = new();
		private readonly TimeOffRequest.SortStatus _statusSorter = new();
		private readonly TimeOffRequest.SortStatusReverse _statusReverseSorter = new();

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
		
		public void SortByTimeOffStart() => timeOffRequests?.Sort(_timeOffStartSorter);
		
		public void SortByTimeOffStartReverse() => timeOffRequests?.Sort(_timeOffStartReverseSorter);
		
		public void SortByHours() => timeOffRequests?.Sort(_hoursSorter);
		
		public void SortByHoursReverse() => timeOffRequests?.Sort(_hoursReverseSorter);
		
		public void SortByDateTimeRequested() => timeOffRequests?.Sort(_dateTimeRequestedSorter);
		
		public void SortByDateTimeRequestedReverse() => timeOffRequests?.Sort(_dateTimeRequestedReverseSorter);
		
		public void SortByStatus() => timeOffRequests?.Sort(_statusSorter);
		
		public void SortByStatusReverse() => timeOffRequests?.Sort(_statusReverseSorter);
	}
}