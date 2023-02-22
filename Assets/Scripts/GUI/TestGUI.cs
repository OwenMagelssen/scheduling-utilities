using System.Globalization;
using UI.Tables;
using UnityEngine;

namespace SchedulingUtilities
{
    public class TestGUI : MonoBehaviour
    {
        public TimeOffRequestReport report;
        public TableLayout tableLayout;

        private void Start()
        {
            if (report == null) return;
            if (tableLayout == null) return;
            tableLayout.ClearRows();
            var culture = new CultureInfo("en-US");

            foreach (var request in report.timeOffRequests)
            {
                var row = tableLayout.AddRow();
                row.preferredHeight = 128;
                row.AddCell(ReportUtilities.CreateStringCell(request.EmployeeName));
                row.AddCell(ReportUtilities.CreateStringCell(request.JobTitle.ToString()));
                row.AddCell(ReportUtilities.CreateStringCell(request.TimeOffStart.ToString(culture)));
                row.AddCell(ReportUtilities.CreateStringCell(request.Hours.ToString(culture)));
                row.AddCell(ReportUtilities.CreateStringCell(request.RequestedOn.ToString(culture)));
                row.AddCell(ReportUtilities.CreateStringCell(request.Status.ToString()));
            }

            float width = (float)Screen.width / tableLayout.ColumnWidths.Count;
            float tableHeight = 0;
            
            for (int i = 0; i < tableLayout.ColumnWidths.Count; i++)
                tableLayout.ColumnWidths[i] = width;
            
            tableLayout.CalculateLayoutInputHorizontal();
            
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
