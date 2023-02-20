using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEditor;
using UnityEngine;

namespace SchedulingUtilities
{
    public static class ReportUtilities
    {
        [MenuItem("Assets/Create/Scheduling Time Off Request Table", priority = 50)]
        public static void CsvToTableGUI()
        {
            string selectionPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
            FileAttributes attr = File.GetAttributes(selectionPath);
            bool isDirectory = (attr & FileAttributes.Directory) == FileAttributes.Directory;

            if (string.IsNullOrEmpty(selectionPath) || isDirectory)
            {
                Debug.Log("A CSV file must be selected to generate a table.");
                return;
            }
            
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false
            };

            var path = selectionPath;
            using var streamReader = File.OpenText(path);
            using var csvReader = new CsvReader(streamReader, csvConfig);

            while (csvReader.Read())
            {
                string row = string.Empty;
                
                for (int i = 0; csvReader.TryGetField(i, out string value); i++)
                {
                    row += value;
                }

                Debug.Log(row);
            }
        }
    }
}
