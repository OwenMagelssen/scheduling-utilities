using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEngine;

namespace SchedulingUtilities
{
    public static class ReportUtilities
    {
        public static void CsvToTableGUI(string csvFilePath, IReportDataDescriptor reportDescriptor)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false
            };

            var path = Application.dataPath + "TimeOffReport.csv";
            using var streamReader = File.OpenText(path);
            using var csvReader = new CsvReader(streamReader, csvConfig);

            string value;

            while (csvReader.Read())
            {
                for (int i = 0; csvReader.TryGetField<string>(i, out value); i++)
                {
                    Debug.Log(value);
                }
            }
        }
    }
}
