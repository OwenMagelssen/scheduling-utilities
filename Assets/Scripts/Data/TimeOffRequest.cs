using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace SchedulingUtilities
{
	[Serializable]
	public class TimeOffRequest : ReportItem, IComparable, ISerializationCallbackReceiver
	{
		public string EmployeeName;
		public JobTitle JobTitle;
		public DateTime TimeOffStart;
		public float Hours;
		public DateTime RequestedOn;
		public Status Status;

		[SerializeField] private long _timeOffStartTicks;
		[SerializeField] private long _requestedOnTicks;
		
		public string AsJson()
		{
			var settings = new JsonSerializerSettings();
			settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			return JsonConvert.SerializeObject(this, settings);
		}
		
		public void OnBeforeSerialize()
		{
			_timeOffStartTicks = TimeOffStart.Ticks;
			_requestedOnTicks = RequestedOn.Ticks;
		}

		public void OnAfterDeserialize()
		{
			TimeOffStart = new DateTime(_timeOffStartTicks);
			RequestedOn = new DateTime(_requestedOnTicks);
		}
		
		public int CompareTo(object obj)
		{
			throw new NotImplementedException();
		}
		
		#region IComparer Implementations
		
		public class SortName : IComparer<TimeOffRequest>
		{
			public int Compare(TimeOffRequest x, TimeOffRequest y)
			{
				if (x == null && y == null)
					return 0;

				if (x == null)
					return -1;

				if (y == null)
					return 1;

				return (new CaseInsensitiveComparer()).Compare(x.EmployeeName, y.EmployeeName);
			}
		}
		
		public class SortNameReverse : IComparer<TimeOffRequest>
		{
			public int Compare(TimeOffRequest x, TimeOffRequest y)
			{
				if (x == null && y == null)
					return 0;

				if (x == null)
					return 1;

				if (y == null)
					return -1;

				return (new CaseInsensitiveComparer()).Compare(y.EmployeeName, x.EmployeeName);
			}
		}
		
		public class SortTitle : IComparer<TimeOffRequest>
		{
			public int Compare(TimeOffRequest x, TimeOffRequest y)
			{
				if (x == null && y == null)
					return 0;

				if (x == null)
					return -1;

				if (y == null)
					return 1;

				int compare = (new CaseInsensitiveComparer()).Compare(x.JobTitle, y.JobTitle);
				return compare == 0
					? (new CaseInsensitiveComparer()).Compare(x.EmployeeName, y.EmployeeName)
					: compare;
			}
		}
		
		public class SortTitleReverse : IComparer<TimeOffRequest>
		{
			public int Compare(TimeOffRequest x, TimeOffRequest y)
			{
				if (x == null && y == null)
					return 0;

				if (x == null)
					return 1;

				if (y == null)
					return -1;

				int compare = (new CaseInsensitiveComparer()).Compare(y.JobTitle, x.JobTitle);
				return compare == 0
					? (new CaseInsensitiveComparer()).Compare(y.EmployeeName, x.EmployeeName)
					: compare;
			}
		}
		
		public class SortTimeOffStart : IComparer<TimeOffRequest>
		{
			public int Compare(TimeOffRequest x, TimeOffRequest y)
			{
				if (x == null && y == null)
					return 0;

				if (x == null)
					return -1;

				if (y == null)
					return 1;

				int compare = x.TimeOffStart.CompareTo(y.TimeOffStart);
				return compare == 0
					? (new CaseInsensitiveComparer()).Compare(x.EmployeeName, y.EmployeeName)
					: compare;
			}
		}
		
		public class SortTimeOffStartReverse : IComparer<TimeOffRequest>
		{
			public int Compare(TimeOffRequest x, TimeOffRequest y)
			{
				if (x == null && y == null)
					return 0;

				if (x == null)
					return 1;

				if (y == null)
					return -1;

				int compare = y.TimeOffStart.CompareTo(x.TimeOffStart);
				return compare == 0
					? (new CaseInsensitiveComparer()).Compare(y.EmployeeName, x.EmployeeName)
					: compare;
			}
		}
		
		public class SortHours : IComparer<TimeOffRequest>
		{
			public int Compare(TimeOffRequest x, TimeOffRequest y)
			{
				if (x == null && y == null)
					return 0;

				if (x == null)
					return -1;

				if (y == null)
					return 1;

				int compare = x.Hours.CompareTo(y.Hours);
				return compare == 0
					? (new CaseInsensitiveComparer()).Compare(x.EmployeeName, y.EmployeeName)
					: compare;
			}
		}
		
		public class SortHoursReverse : IComparer<TimeOffRequest>
		{
			public int Compare(TimeOffRequest x, TimeOffRequest y)
			{
				if (x == null && y == null)
					return 0;

				if (x == null)
					return 1;

				if (y == null)
					return -1;

				int compare = y.Hours.CompareTo(x.Hours);
				return compare == 0
					? (new CaseInsensitiveComparer()).Compare(y.EmployeeName, x.EmployeeName)
					: compare;
			}
		}
		
		public class SortDateTimeRequested : IComparer<TimeOffRequest>
		{
			public int Compare(TimeOffRequest x, TimeOffRequest y)
			{
				if (x == null && y == null)
					return 0;

				if (x == null)
					return -1;

				if (y == null)
					return 1;

				int compare = x.RequestedOn.CompareTo(y.RequestedOn);
				return compare == 0
					? (new CaseInsensitiveComparer()).Compare(x.EmployeeName, y.EmployeeName)
					: compare;
			}
		}
		
		public class SortDateTimeRequestedReverse : IComparer<TimeOffRequest>
		{
			public int Compare(TimeOffRequest x, TimeOffRequest y)
			{
				if (x == null && y == null)
					return 0;

				if (x == null)
					return 1;

				if (y == null)
					return -1;

				int compare = y.RequestedOn.CompareTo(x.RequestedOn);
				return compare == 0
					? (new CaseInsensitiveComparer()).Compare(y.EmployeeName, x.EmployeeName)
					: compare;
			}
		}
		
		public class SortStatus : IComparer<TimeOffRequest>
		{
			public int Compare(TimeOffRequest x, TimeOffRequest y)
			{
				if (x == null && y == null)
					return 0;

				if (x == null)
					return -1;

				if (y == null)
					return 1;

				int compare = x.Status.CompareTo(y.Status);
				return compare == 0
					? (new CaseInsensitiveComparer()).Compare(x.EmployeeName, y.EmployeeName)
					: compare;
			}
		}
		
		public class SortStatusReverse : IComparer<TimeOffRequest>
		{
			public int Compare(TimeOffRequest x, TimeOffRequest y)
			{
				if (x == null && y == null)
					return 0;

				if (x == null)
					return 1;

				if (y == null)
					return -1;

				int compare = y.Status.CompareTo(x.Status);
				return compare == 0
					? (new CaseInsensitiveComparer()).Compare(y.EmployeeName, x.EmployeeName)
					: compare;
			}
		}
		
		#endregion
	}
}