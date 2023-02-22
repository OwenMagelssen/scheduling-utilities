using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace SchedulingUtilities
{
    public static class ReportUtilities
    {
        public static RectTransform CreateStringCell(string value)
        {
            var go = new GameObject();
            var rt = go.AddComponent<RectTransform>();
            rt.pivot = Vector2.zero;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            var text = rt.AddComponent<TextMeshProUGUI>();
            text.text = value;
            return rt;
        }
        
        
        
        // [MenuItem("Assets/Create/Scheduling Time Off Request Table", priority = 50)]
        // public static void CsvToTableGUI()
        // {
        //     string selectionPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        //     FileAttributes attr = File.GetAttributes(selectionPath);
        //     bool isDirectory = (attr & FileAttributes.Directory) == FileAttributes.Directory;
        //
        //     if (string.IsNullOrEmpty(selectionPath) || isDirectory)
        //     {
        //         Debug.Log("A CSV file must be selected to generate a table.");
        //         return;
        //     }
        //     
        //     var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
        //     {
        //         HasHeaderRecord = false
        //     };
        //
        //     Debug.Log(selectionPath);
        //     using var streamReader = File.OpenText(selectionPath);
        //     using var csvReader = new CsvReader(streamReader, csvConfig);
        //
        //     while (csvReader.Read())
        //     {
        //         string row = string.Empty;
        //         
        //         for (int i = 0; csvReader.TryGetField(i, out string value); i++)
        //         {
        //             row += value;
        //         }
        //
        //         Debug.Log(row);
        //     }
        // }
    }
}
