using System;
using UnityEngine;

namespace SchedulingUtilities
{
	[Serializable]
	public class TimeOffRequest : ReportItem, ISerializationCallbackReceiver
	{
		public string EmployeeName;
		public JobTitle JobTitle;
		public DateTime TimeOffStart;
		public float Hours;
		public DateTime RequestedOn;
		public Status Status;

		[SerializeField] private long _timeOffStartTicks;
		[SerializeField] private long _requestedOnTicks;
		
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
	}
}