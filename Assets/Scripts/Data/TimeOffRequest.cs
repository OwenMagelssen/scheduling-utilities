using System;
using System.Collections;
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
		
		private class SortNameAlphabetical : IComparer
		{
			int IComparer.Compare(object a, object b)
			{
				TimeOffRequest r1 = (TimeOffRequest)a;
				TimeOffRequest r2 = (TimeOffRequest)b;

				if (r1 == null && r2 == null)
					return 0;

				if (r1 == null)
					return -1;

				if (r2 == null)
					return 1;

				return string.Compare(r1.EmployeeName, r2.EmployeeName, StringComparison.CurrentCultureIgnoreCase);
			}
		}
		
		private class SortNameReverseAlphabetical : IComparer
		{
			int IComparer.Compare(object a, object b)
			{
				TimeOffRequest r1 = (TimeOffRequest)a;
				TimeOffRequest r2 = (TimeOffRequest)b;

				if (r1 == null && r2 == null)
					return 0;

				if (r1 == null)
					return 1;

				if (r2 == null)
					return -1;

				return -1 * string.Compare(r1.EmployeeName, r2.EmployeeName, StringComparison.CurrentCultureIgnoreCase);
			}
		}
	}
}