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
	}
}