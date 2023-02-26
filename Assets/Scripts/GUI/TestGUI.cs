using System;
using System.Collections.Generic;
using System.Globalization;
using UI.Tables;
using UnityEngine;
using TMPro;

namespace SchedulingUtilities
{
    public class TestGUI : MonoBehaviour
    {
        public TextMeshProUGUI stringCellPrefab;
        public TimeOffRequestReport report;
        public TableLayout tableLayout;
        public Vector2 padding = Vector2.one * 24.0f;

        private RectTransform _rectTransform;
        private List<TextMeshProUGUI> _allCellTexts = new();

        private float AddStringCellGetWidth(TableRow row, TextMeshProUGUI text, string value)
        {
            _allCellTexts.Add(text);
            var rt = text.GetComponent<RectTransform>();
            rt.pivot = Vector2.zero;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            text.text = value;
            text.ForceMeshUpdate();
            row.AddCell(rt);
            return text.preferredWidth;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            if (report == null) return;
            if (tableLayout == null) return;
            tableLayout.ClearRows();
            var culture = new CultureInfo("en-US");
            var columnWidths = new List<float>() { 0, 0, 0, 0, 0, 0 };

            foreach (var request in report.timeOffRequests)
            {
                var row = tableLayout.AddRow();
                columnWidths[0] = Mathf.Max(columnWidths[0], AddStringCellGetWidth(row, Instantiate(stringCellPrefab), request.EmployeeName));
                columnWidths[1] = Mathf.Max(columnWidths[1], AddStringCellGetWidth(row, Instantiate(stringCellPrefab), request.JobTitle.ToString()));
                columnWidths[2] = Mathf.Max(columnWidths[2], AddStringCellGetWidth(row, Instantiate(stringCellPrefab), request.TimeOffStart.ToString(culture)));
                columnWidths[3] = Mathf.Max(columnWidths[3], AddStringCellGetWidth(row, Instantiate(stringCellPrefab), request.Hours.ToString(culture)));
                columnWidths[4] = Mathf.Max(columnWidths[4], AddStringCellGetWidth(row, Instantiate(stringCellPrefab), request.RequestedOn.ToString(culture)));
                columnWidths[5] = Mathf.Max(columnWidths[5], AddStringCellGetWidth(row, Instantiate(stringCellPrefab), request.Status.ToString()));
            }

            float columnWidthSum = 0;
            columnWidths.ForEach(i => columnWidthSum += i + padding.x);
            float cellScale = _rectTransform.rect.width / columnWidthSum;
            
            for (int i = 0; i < columnWidths.Count; i++)
                columnWidths[i] = (columnWidths[i] + padding.x) * cellScale;

            for (int i = 0; i < _allCellTexts.Count; i++)
                _allCellTexts[i].fontSize *= cellScale;
            
            tableLayout.ColumnWidths = columnWidths;
            tableLayout.CalculateLayoutInputHorizontal();

            tableLayout.Rows.ForEach(row => row.preferredHeight = (stringCellPrefab.fontSize + padding.y) * cellScale);
            float tableHeight = 0;
            
            for (int i = 0; i < tableLayout.Rows.Count; i++)
                tableHeight += tableLayout.Rows[i].actualHeight;

            var rt = tableLayout.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, tableHeight);
        }

        private void DoImmediateModeGUI()
        {
            var culture = new CultureInfo("en-US");
            
            foreach (var request in report.timeOffRequests)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(request.EmployeeName);
                GUILayout.Label(request.JobTitle.ToString());
                GUILayout.Label(request.TimeOffStart.ToString(culture));
                GUILayout.Label(request.Hours.ToString(culture));
                GUILayout.Label(request.RequestedOn.ToString(culture));
                GUILayout.Label(request.Status.ToString());
                GUILayout.EndHorizontal();
            }
        }

        // public void OnGUI()
        // {
        //     if (report == null) return;
        //     if (tableLayout == null) return;
        //     
        // }
    }
}
