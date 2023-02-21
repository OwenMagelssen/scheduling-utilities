using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace SchedulingUtilities
{
	public interface IReport
	{
		public EtmReportItemProcessor ItemProcessor { get; }
		public bool CsvHasHeader { get; }

		public List<T> ProcessEtmReport<T>(string csvFilePath) where T : ReportItem
		{
			if (string.IsNullOrEmpty(csvFilePath)) return null;
			if (Path.GetExtension(csvFilePath) != ".csv") return null;
			var items = new List<T>();
			
			var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
			{
				HasHeaderRecord = false
			};

			using var streamReader = File.OpenText(csvFilePath);
			using var csvReader = new CsvReader(streamReader, csvConfig);
			var row = new List<string>();
			int rowsProcessed = 0;

			while (csvReader.Read())
			{
				row.Clear();
                
				for (int i = 0; csvReader.TryGetField(i, out string value); i++)
				{
					row.Add(value);
				}

				rowsProcessed++;
				
				if (rowsProcessed > 1 || !CsvHasHeader)
					items.Add(ItemProcessor.CreateReport<T>(row));
			}

			return items;
		}
	}
}