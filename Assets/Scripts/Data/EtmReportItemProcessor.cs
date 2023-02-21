using System.Collections.Generic;

namespace SchedulingUtilities
{
	public abstract class EtmReportItemProcessor
	{
		public abstract T CreateReport<T>(List<string> values) where T : ReportItem;
	}
}