using System;
using System.Collections.Generic;
using System.Globalization;
using UI.Tables;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace SchedulingUtilities
{
    public class TestGUI : MonoBehaviour
    {
        public enum Column
        {
            Name,
            Title,
            Start,
            Hours,
            Requested,
            Status
        }

        public ColumnHeader columnHeaderPrefab;
        public CellData stringCellPrefab;
        public TimeOffRequestReport report;
        public RectTransform tableHeaderRect;
        // public RectTransform tableContentsRect;
        public TableLayout tableLayout;
        public TimeOffRequestInspector requestInspector;
        public RectTransform verticalScrollbar;
        public RectOffset tablePadding;
        public float cellSpacing;
        public float cellPadding;
        public Color rowColor = Color.white;
        
        [Header("Row Interaction Settings")]
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
        private List<float> _columnWidths = new () { 0, 0, 0, 0, 0, 0 };
        private List<ColumnHeader> _columnHeaders = new();
        private List<TextMeshProUGUI> _allCellTexts = new();
        private Dictionary<TimeOffRequest, TableRow> _rowDictionary = new();
        private float _scrollbarWidth;
        private RectOffset _noPadding;
        private TimeOffRequestReportSorter _sorter;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _noPadding = new (0, 0, 0, 0);
        }

        private void OnEnable()
        {
            var disableEvent = verticalScrollbar.GetComponent<EventOnDisable>();
            disableEvent.onDisable += RecalculateLayout;
        }
        
        
        private void OnDisable()
        {
            var disableEvent = verticalScrollbar.GetComponent<EventOnDisable>();
            disableEvent.onDisable -= RecalculateLayout;
        }

        private void Start()
        {
// #if UNITY_EDITOR
//             CreateTable();
// #endif
        }
        
        public void GenerateTestEmailFile()
        {
            if (report == null) return;
            GenerateEmail.FromTimeOffRequestReport(report);
        }

        public void CreateNewReportAndTable(string pathToCsv)
        {
            report = ScriptableObject.CreateInstance<TimeOffRequestReport>();
            report.CreateReport(pathToCsv);
            CreateTable();
        }
        
        private float AddCellGetWidth(TableRow row, CellData cellData)
        {
            if (cellData.CellDataType == CellDataType.None)
            {
                Debug.LogWarning("Trying to add cell with uninitialized data.");
                return 0;
            }
            
            _allCellTexts.Add(cellData.Text);
            var rt = cellData.GetComponent<RectTransform>();
            rt.pivot = Vector2.zero;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            row.AddCell(rt);
            cellData.Text.margin = Vector4.one * cellPadding;
            cellData.Text.ForceMeshUpdate();
            return cellData.Text.preferredWidth + cellPadding * 2.0f;
        }

        private float AddCellGetWidth(TableRow row, CellData cellData, string value)
        {
            cellData.SetData(value);
            return AddCellGetWidth(row, cellData);
        }
        
        private float AddCellGetWidth(TableRow row, CellData cellData, float value)
        {
            cellData.SetData(value);
            return AddCellGetWidth(row, cellData);
        }
        
        private float AddCellGetWidth(TableRow row, CellData cellData, DateTime value)
        {
            cellData.SetData(value);
            return AddCellGetWidth(row, cellData);
        }

        private float AddCellGetWidth(TableRow row, CellData cellData, Enum value, Color? cellColor = null)
        {
            cellData.SetData(value);
            
            if (cellColor != null)
                cellData.SetColor(cellColor.Value);
            
            return AddCellGetWidth(row, cellData);
        }

        private ColumnHeader CreateColumnHeader(string label, Action sortFunction, Column column)
        {
            var header = Instantiate(columnHeaderPrefab, tableHeaderRect);
            header.Label.text = label;
            header.Button.onClick.AddListener(new UnityAction(sortFunction));
            header.SortIndicator.Setup(column);
            _sorter.OnSorted += header.SortIndicator.SetIndicator;
            header.RectTransform.pivot = Vector2.zero;
            header.RectTransform.anchorMin = header.RectTransform.anchorMax = Vector2.zero;
            
            // var rt = header.GetComponent<RectTransform>();
            // rt.pivot = Vector2.zero;
            // rt.anchorMin = Vector2.zero;
            // rt.anchorMax = Vector2.one;
            // rt.sizeDelta = Vector2.zero;
            var padding = Vector4.one * cellPadding;
            padding.z += header.SortIndicator.indicatorImage.rectTransform.rect.width + cellPadding;
            header.Label.margin = padding;
            header.SortIndicator.indicatorImage.rectTransform.anchoredPosition = new Vector2(-cellPadding, 0);
            header.Label.ForceMeshUpdate();
            header.Width = header.Label.preferredWidth + cellPadding * 3.0f + header.SortIndicator.indicatorImage.rectTransform.rect.width;
            
            return header;
        }

        private Color? GetStatusColor(Status status)
        {
            Color statusColor = (status) switch
            {
                Status.Denied => Color.red,
                Status.Approved => Color.green,
                Status.Pending => Color.yellow,
                _ => Color.clear
            };

            return statusColor;
        }

        [UnityEngine.ContextMenu("Create Table")]
        private void CreateTable()
        {
            if (report == null) return;
            if (tableLayout == null) return;
            
            _sorter = new(report);
            _sorter.OnSorted += (sortType) => UpdateRowOrder();
            _rowDictionary.Clear();
            tableLayout.DestroyRows();
            tableLayout.RowBackgroundColor = rowColor;
            tableLayout.padding = tablePadding;
            tableLayout.CellPadding = _noPadding;
            tableLayout.CellSpacing = cellSpacing;
            
            var culture = new CultureInfo("en-US");

            _columnHeaders.Clear();
            _columnHeaders.Add(CreateColumnHeader("Name", _sorter.SortByName, Column.Name));
            _columnHeaders.Add(CreateColumnHeader("Title", _sorter.SortByTitle, Column.Title));
            _columnHeaders.Add(CreateColumnHeader("Time Off Start", _sorter.SortByTimeOffStart, Column.Start));
            _columnHeaders.Add(CreateColumnHeader("Hours", _sorter.SortByHours, Column.Hours));
            _columnHeaders.Add(CreateColumnHeader("Requested On", _sorter.SortByDateTimeRequested, Column.Requested));
            _columnHeaders.Add(CreateColumnHeader("Status", _sorter.SortByStatus, Column.Status));
            
            for (int i = 0; i < _columnWidths.Count; i++)
                _columnWidths[i] = _columnHeaders[i].Width;

            for (int i = 0; i < report.timeOffRequests.Count; i++)
            {
                var request = report.timeOffRequests[i];
                TableRow row = tableLayout.AddRow();
                _rowDictionary.TryAdd(request, row);
                
                var button = row.gameObject.AddComponent<Button>();
                var selectable = (Selectable)button;
                selectable.colors = rowSelectableColors;
                button.onClick.AddListener(() => SelectRequest(request));
                
                _columnWidths[0] = Mathf.Max(_columnWidths[0], AddCellGetWidth(row, Instantiate(stringCellPrefab), request.EmployeeName));
                _columnWidths[1] = Mathf.Max(_columnWidths[1], AddCellGetWidth(row, Instantiate(stringCellPrefab), request.JobTitle));
                _columnWidths[2] = Mathf.Max(_columnWidths[2], AddCellGetWidth(row, Instantiate(stringCellPrefab), request.TimeOffStart));
                _columnWidths[3] = Mathf.Max(_columnWidths[3], AddCellGetWidth(row, Instantiate(stringCellPrefab), request.Hours));
                _columnWidths[4] = Mathf.Max(_columnWidths[4], AddCellGetWidth(row, Instantiate(stringCellPrefab), request.RequestedOn));

                Color? statusColor = GetStatusColor(request.Status);
                _columnWidths[5] = Mathf.Max(_columnWidths[5], AddCellGetWidth(row, Instantiate(stringCellPrefab), request.Status, statusColor));
            }

            _sorter.SortByName();
            RecalculateLayout();
        }

        public void RedrawRequest(TimeOffRequest request)
        {
            var row = _rowDictionary[request];
            row.Cells[0].GetComponentInChildren<CellData>().SetData(request.EmployeeName);
            row.Cells[1].GetComponentInChildren<CellData>().SetData(request.JobTitle);
            row.Cells[2].GetComponentInChildren<CellData>().SetData(request.TimeOffStart);
            row.Cells[3].GetComponentInChildren<CellData>().SetData(request.Hours);
            row.Cells[4].GetComponentInChildren<CellData>().SetData(request.RequestedOn);
            var statusData = row.Cells[5].GetComponentInChildren<CellData>();
            statusData.SetData(request.Status);
            var statusColor = GetStatusColor(request.Status);
            
            if (statusColor != null)
                statusData.SetColor(statusColor.Value);
        }

        [UnityEngine.ContextMenu("Recalculate Layout")]
        public void RecalculateLayout()
        {
            // if (Application.q)
            _scrollbarWidth = verticalScrollbar == null || !verticalScrollbar.gameObject.activeInHierarchy ? 0 : verticalScrollbar.rect.width;
            
            float columnWidthSum = 0;
            _columnWidths.ForEach(width => columnWidthSum += (width + cellSpacing));
            float cellScale = _rectTransform.rect.width / (columnWidthSum + _scrollbarWidth + cellSpacing);

            List<float> scaledColumnWidths = new List<float>(6);
            
            for (int i = 0; i < _columnWidths.Count; i++)
                scaledColumnWidths.Add(_columnWidths[i] * cellScale);

            float scaledFontSize = stringCellPrefab.Text.fontSize * cellScale;
            
            for (int i = 0; i < _allCellTexts.Count; i++)
                _allCellTexts[i].fontSize = scaledFontSize - cellPadding * 2.0f;
            
            tableLayout.ColumnWidths = scaledColumnWidths;

            // tableHeaderRect.sizeDelta = new Vector2(columnWidthSum * cellScale, tableHeaderRect.rect.height);
            float xHeaderPos = 0;
            
            for (int i = 0; i < scaledColumnWidths.Count; i++)
            {
                _columnHeaders[i].RectTransform.SetParent(tableHeaderRect);
                _columnHeaders[i].RectTransform.anchoredPosition = new Vector2(xHeaderPos, 0);
                _columnHeaders[i].RectTransform.sizeDelta =
                    new Vector2(scaledColumnWidths[i], tableHeaderRect.rect.height);
                _columnHeaders[i].Label.fontSize = scaledFontSize - cellPadding * 2.0f;
                xHeaderPos += scaledColumnWidths[i];
            }
            
            tableLayout.CalculateLayoutInputHorizontal();

            tableLayout.Rows.ForEach(row => row.preferredHeight = scaledFontSize);
            float tableHeight = 50.0f; // default extra height
            
            for (int i = 0; i < tableLayout.Rows.Count; i++)
                tableHeight += tableLayout.Rows[i].preferredHeight + cellSpacing;

            var rt = tableLayout.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, tableHeight);
        }

        private void UpdateRowOrder()
        {
            tableLayout.ClearRows();

            for (int i = 0; i < report.timeOffRequests.Count; i++)
                tableLayout.AddRow(_rowDictionary[report.timeOffRequests[i]]);
        }

        private void SelectRequest(TimeOffRequest request)
        {
            requestInspector.gameObject.SetActive(true);
            requestInspector.TimeOffRequest = request;
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
        //     DoImmediateModeGUI();
        // }
    }
}
