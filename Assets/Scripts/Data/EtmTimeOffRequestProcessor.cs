using System;
using System.Collections.Generic;
using System.Globalization;

namespace SchedulingUtilities
{
	public class EtmTimeOffRequestProcessor : EtmReportItemProcessor
	{
		public override T CreateReport<T>(List<string> values)
		{
			var timeOffRequest = new TimeOffRequest();
			timeOffRequest.EmployeeName = values[2];
			timeOffRequest.JobTitle = values[3] switch
			{
				"Security Officer" => JobTitle.SecurityOfficer,
				"Security Working Supervisor" => JobTitle.SecuritySupervisor,
				_ => JobTitle.Other
			};
			CultureInfo culture = new CultureInfo("en-US");   
			timeOffRequest.TimeOffStart = DateTime.Parse($"{values[4]} {values[5]}", culture);
			float.TryParse(values[6], out float dayHours);
			float.TryParse(values[7], out float nightHours);
			timeOffRequest.Hours = dayHours + nightHours;
			timeOffRequest.RequestedOn = DateTime.Parse(values[8], culture);
			timeOffRequest.Status = Status.Pending;
			return timeOffRequest as T;
		}
	}
}