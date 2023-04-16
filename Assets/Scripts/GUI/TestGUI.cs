using System;
using System.Collections.Generic;
using System.Globalization;
using UI.Tables;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SchedulingUtilities
{
    public class TestGUI : MonoBehaviour
    {
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
        private List<float> _columnWidths = new () { 0, 0, 0, 0, 0, 0 };
        private List<ColumnHeader> _columnHeaders = new();
        private List<TextMeshProUGUI> _allCellTexts = new();
        private Dictionary<TimeOffRequest, TableRow> _rowDictionary = new();
        private float _scrollbarWidth;
        private RectOffset _noPadding;

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

        private float AddEnumCellGetWidth(TableRow row, CellData cellData, Enum value, Color? cellColor = null)
        {
            _allCellTexts.Add(cellData.Text);
            var rt = cellData.GetComponent<RectTransform>();
            rt.pivot = Vector2.zero;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            row.AddCell(rt);
            cellData.Text.margin = Vector4.one * cellPadding;
            cellData.SetColor((cellColor ?? Color.clear));
            cellData.SetData(value);
            cellData.Text.ForceMeshUpdate();
            return cellData.Text.preferredWidth + cellPadding * 2.0f;
        }

        private ColumnHeader CreateColumnHeader(string label, Action sortFunction)
        {
            var header = Instantiate(columnHeaderPrefab, tableHeaderRect);
            header.label.text = label;
            header.button.onClick.AddListener(new UnityAction(sortFunction));
            header.RectTransform.pivot = Vector2.zero;
            header.RectTransform.anchorMin = header.RectTransform.anchorMax = Vector2.zero;
            return header;
        }

        [ContextMenu("Create Table")]
        private void CreateTable()
        {
            if (report == null) return;
            if (tableLayout == null) return;
            
            _rowDictionary.Clear();
            tableLayout.ClearRows();
            tableLayout.RowBackgroundColor = rowColor;
            tableLayout.padding = tablePadding;
            tableLayout.CellPadding = _noPadding;
            tableLayout.CellSpacing = cellSpacing;
            
            var culture = new CultureInfo("en-US");

            for (int i = 0; i < _columnWidths.Count; i++)
                _columnWidths[i] = 0;

            for (int i = 0; i < report.timeOffRequests.Count; i++)
            {
                var request = report.timeOffRequests[i];
                TableRow row = tableLayout.AddRow();
                _rowDictionary.TryAdd(request, row);
                
                var button = row.gameObject.AddComponent<Button>();
                var selectable = (Selectable)button;
                selectable.colors = rowSelectableColors;
                button.onClick.AddListener(() => SelectRequest(request));
                
                _columnWidths[0] = Mathf.Max(_columnWidths[0], AddStringCellGetWidth(row, Instantiate(stringCellPrefab), request.EmployeeName));
                _columnWidths[1] = Mathf.Max(_columnWidths[1], AddEnumCellGetWidth(row, Instantiate(stringCellPrefab), request.JobTitle));
                _columnWidths[2] = Mathf.Max(_columnWidths[2], AddDateTimeCellGetWidth(row, Instantiate(stringCellPrefab), request.TimeOffStart));
                _columnWidths[3] = Mathf.Max(_columnWidths[3], AddFloatCellGetWidth(row, Instantiate(stringCellPrefab), request.Hours));
                _columnWidths[4] = Mathf.Max(_columnWidths[4], AddDateTimeCellGetWidth(row, Instantiate(stringCellPrefab), request.RequestedOn));
                
                Color statusColor = (request.Status) switch
                {
                    Status.Denied => Color.red,
                    Status.Approved => Color.green,
                    Status.Pending => Color.yellow,
                    _ => Color.clear
                };
                
                _columnWidths[5] = Mathf.Max(_columnWidths[5], AddEnumCellGetWidth(row, Instantiate(stringCellPrefab), request.Status, statusColor));
            }

            _columnHeaders.Clear();
            _columnHeaders.Add(CreateColumnHeader("Name", SortByName));
            _columnHeaders.Add(CreateColumnHeader("Title", SortByTitle));
            _columnHeaders.Add(CreateColumnHeader("Time Off Start", SortByTimeOffStart));
            _columnHeaders.Add(CreateColumnHeader("Hours", SortByHours));
            _columnHeaders.Add(CreateColumnHeader("Requested On", SortByDateTimeRequested));
            _columnHeaders.Add(CreateColumnHeader("Status", SortByStatus));
            RecalculateLayout();
        }

        [ContextMenu("Recalculate Layout")]
        public void RecalculateLayout()
        {
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

        private void RedrawRows()
        {
            CreateTable();
        }

        private enum SortType
        {
            Name,
            NameReverse,
            Title,
            TitleReverse,
            TimeOffStart,
            TimeOffStartReverse,
            Hours,
            HoursReverse,
            
        }

        private void SortByName()
        {
            Debug.Log("Sorted by name");
            report.SortByName();
            RedrawRows();
        }

        private void SortByTitle()
        {
            Debug.Log("Sorted by title");
        }

        private void SortByTimeOffStart()
        {
            Debug.Log("Sorted by time off start");
        }

        private void SortByHours()
        {
            Debug.Log("Sorted by hours");
        }

        private void SortByDateTimeRequested()
        {
            Debug.Log("Sorted by time requested");
        }
        
        private void SortByStatus()
        {
            Debug.Log("Sorted by status");
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
        //     
        // }
    }
}
