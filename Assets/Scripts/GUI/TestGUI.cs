using System;
using System.Collections.Generic;
using System.Globalization;
using UI.Tables;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SchedulingUtilities
{
    public class TestGUI : MonoBehaviour
    {
        public CellData stringCellPrefab;
        public TimeOffRequestReport report;
        public TableLayout tableLayout;
        public RectTransform verticalScrollbar;
        public RectOffset tablePadding;
        public float cellSpacing;
        public float cellPadding;
        public Color rowColor = Color.white;
        [Header("Row Selection")]
        public ColorBlock rowSelectableColors = new ColorBlock
        {
            normalColor = Color.white,
            highlightedColor = new Color(130, 200, 255, 255),
            pressedColor = new Color(151, 146, 255, 255),
            selectedColor = new Color(59, 69, 255, 255),
            disabledColor = new Color(173, 173, 173, 255),
            colorMultiplier = 1.0f,
            fadeDuration = 0.2f
        };

        private RectTransform _rectTransform;
        private List<TextMeshProUGUI> _allCellTexts = new();
        private float _scrollbarWidth;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _scrollbarWidth = verticalScrollbar == null ? 0 : verticalScrollbar.rect.width;
            CreateTable();
        }

        private float AddStringCellGetWidth(TableRow row, CellData cellData, string value)
        {
            _allCellTexts.Add(cellData.Text);
            var rt = cellData.GetComponent<RectTransform>();
            rt.pivot = Vector2.zero;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            row.AddCell(rt);
            cellData.Text.margin = Vector4.one * cellPadding;
            cellData.SetData(value);
            cellData.Text.ForceMeshUpdate();
            return cellData.Text.preferredWidth + cellPadding * 2.0f;
        }
        
        private float AddFloatCellGetWidth(TableRow row, CellData cellData, float value)
        {
            _allCellTexts.Add(cellData.Text);
            var rt = cellData.GetComponent<RectTransform>();
            rt.pivot = Vector2.zero;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            row.AddCell(rt);
            cellData.Text.margin = Vector4.one * cellPadding;
            cellData.SetData(value);
            cellData.Text.ForceMeshUpdate();
            return cellData.Text.preferredWidth + cellPadding * 2.0f;
        }
        
        private float AddDateTimeCellGetWidth(TableRow row, CellData cellData, DateTime value)
        {
            _allCellTexts.Add(cellData.Text);
            var rt = cellData.GetComponent<RectTransform>();
            rt.pivot = Vector2.zero;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            row.AddCell(rt);
            cellData.Text.margin = Vector4.one * cellPadding;
            cellData.SetData(value);
            cellData.Text.ForceMeshUpdate();
            return cellData.Text.preferredWidth + cellPadding * 2.0f;
        }
        
        private float AddEnumCellGetWidth(TableRow row, CellData cellData, Enum value)
        {
            _allCellTexts.Add(cellData.Text);
            var rt = cellData.GetComponent<RectTransform>();
            rt.pivot = Vector2.zero;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            row.AddCell(rt);
            cellData.Text.margin = Vector4.one * cellPadding;
            cellData.SetData(value);
            cellData.Text.ForceMeshUpdate();
            return cellData.Text.preferredWidth + cellPadding * 2.0f;
        }

        [ContextMenu("Create Table")]
        private void CreateTable()
        {
            if (report == null) return;
            if (tableLayout == null) return;
            
            tableLayout.ClearRows();
            tableLayout.RowBackgroundColor = rowColor;
            tableLayout.padding = tablePadding;
            // tableLayout.CellPadding = cellPadding;
            tableLayout.CellSpacing = cellSpacing;
            
            var culture = new CultureInfo("en-US");
            var columnWidths = new List<float>() { 0, 0, 0, 0, 0, 0 };

            foreach (var request in report.timeOffRequests)
            {
                TableRow row = tableLayout.AddRow();
                columnWidths[0] = Mathf.Max(columnWidths[0], AddStringCellGetWidth(row, Instantiate(stringCellPrefab), request.EmployeeName));
                columnWidths[1] = Mathf.Max(columnWidths[1], AddEnumCellGetWidth(row, Instantiate(stringCellPrefab), request.JobTitle));
                columnWidths[2] = Mathf.Max(columnWidths[2], AddDateTimeCellGetWidth(row, Instantiate(stringCellPrefab), request.TimeOffStart));
                columnWidths[3] = Mathf.Max(columnWidths[3], AddFloatCellGetWidth(row, Instantiate(stringCellPrefab), request.Hours));
                columnWidths[4] = Mathf.Max(columnWidths[4], AddDateTimeCellGetWidth(row, Instantiate(stringCellPrefab), request.RequestedOn));
                columnWidths[5] = Mathf.Max(columnWidths[5], AddEnumCellGetWidth(row, Instantiate(stringCellPrefab), request.Status));
            }

            float columnWidthSum = 0;
            columnWidths.ForEach(width => columnWidthSum += (width + cellSpacing));
            float cellScale = _rectTransform.rect.width / (columnWidthSum + _scrollbarWidth + cellSpacing);
            
            for (int i = 0; i < columnWidths.Count; i++)
                columnWidths[i] *= cellScale;

            float scaledFontSize = stringCellPrefab.Text.fontSize * cellScale;
            
            for (int i = 0; i < _allCellTexts.Count; i++)
                _allCellTexts[i].fontSize = scaledFontSize - cellPadding * 2.0f;
            
            tableLayout.ColumnWidths = columnWidths;
            tableLayout.CalculateLayoutInputHorizontal();

            tableLayout.Rows.ForEach(row => row.preferredHeight = scaledFontSize);
            float tableHeight = 0;
            
            for (int i = 0; i < tableLayout.Rows.Count; i++)
                tableHeight += tableLayout.Rows[i].preferredHeight + cellSpacing;

            var rt = tableLayout.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, tableHeight);

            for (int i = 0; i < tableLayout.Rows.Count; i++)
            {
                var button = tableLayout.Rows[i].gameObject.AddComponent<Button>();
                var selectable = button as Selectable;
                selectable.colors = rowSelectableColors;
                int index = i;
                button.onClick.AddListener(() => SelectRequest(report.timeOffRequests[index]));
            }
        }

        private void SelectRequest(TimeOffRequest request)
        {
            
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
