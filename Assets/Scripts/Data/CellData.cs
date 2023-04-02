using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SchedulingUtilities
{
    public enum CellDataType
    {
        StringData,
        FloatData,
        DateTimeData,
        EnumData
    }
    
    public class CellData : MonoBehaviour
    {
        [SerializeField] private Image image;
        public TextMeshProUGUI Text => text;
        [SerializeField] private TextMeshProUGUI text;
        public RectTransform RectTransform => _rectTransform;
        private RectTransform _rectTransform;

        public CellDataType CellDataType => _cellDataType;
        private CellDataType _cellDataType;

        public string DisplayName => _dataDisplayString;
        private string _dataDisplayString;

        public string StringData => _stringData;
        private string _stringData;
        public float FloatData => _floatData;
        private float _floatData;
        public DateTime DateTimeData => _dateTimeData;
        private DateTime _dateTimeData;
        public int EnumValueData => _enumData;
        private int _enumData;
        
        private CultureInfo _culture = new CultureInfo("en-US");

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetData(string data)
        {
            _cellDataType = CellDataType.StringData;
            _stringData = data;
            _dataDisplayString = data;
            text.text = _dataDisplayString;
        }

        public void SetData(float data)
        {
            _cellDataType = CellDataType.FloatData;
            _floatData = data;
            _dataDisplayString = data.ToString(_culture);
            text.text = _dataDisplayString;
        }

        public void SetData(DateTime data)
        {
            _cellDataType = CellDataType.DateTimeData;
            _dateTimeData = data;
            _dataDisplayString = data.ToString(_culture);
            text.text = _dataDisplayString;
        }

        public void SetData(Enum data)
        {
            _cellDataType = CellDataType.EnumData;
            _enumData = (int)(object)data; //lol
            _dataDisplayString = Regex.Replace(data.ToString(), "([A-Z])", " $1").Trim();
            text.text = _dataDisplayString;
        }

        public void SetColor(Color color)
        {
            image.color = color;
        }
    }
}
