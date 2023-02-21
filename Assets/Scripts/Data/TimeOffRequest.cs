using System;

namespace SchedulingUtilities
{
	[Serializable]
	public class TimeOffRequest : ReportItem
	{
		public string EmployeeName;
		public JobTitle JobTitle;
		public DateTime TimeOffStart;
		public float Hours;
		public DateTime RequestedOn;
		public Status Status;
	}
}