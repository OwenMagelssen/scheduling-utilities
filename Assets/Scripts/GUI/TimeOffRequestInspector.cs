using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

namespace SchedulingUtilities
{
    public class TimeOffRequestInspector : MonoBehaviour
    {
        public TimeOffRequest TimeOffRequest
        {
            get => _timeOffRequest;
            set
            {
                _timeOffRequest = value;
                SetNewRequest(_timeOffRequest);
            }
        }

        private TimeOffRequest _timeOffRequest;

        public TestGUI testGUI;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI timeOffStartText;
        public TextMeshProUGUI hoursText;
        public TextMeshProUGUI dateTimeRequestedText;
        public TMP_Dropdown statusDropdown;
        
        private CultureInfo _culture = new CultureInfo("en-US");

        private void SetStatusValue(int value)
        {
            _timeOffRequest.Status = (Status)value;
            testGUI.RedrawRequest(_timeOffRequest);
        }

        private void SetNewRequest(TimeOffRequest request)
        {
            statusDropdown.onValueChanged.RemoveAllListeners();
            nameText.text = request.EmployeeName;
            titleText.text = Regex.Replace(request.JobTitle.ToString(), "([A-Z])", " $1").Trim();
            timeOffStartText.text = request.TimeOffStart.ToString(_culture);
            hoursText.text = request.Hours.ToString(_culture);
            dateTimeRequestedText.text = request.RequestedOn.ToString(_culture);
            statusDropdown.value = (int)request.Status;
            statusDropdown.onValueChanged.AddListener(SetStatusValue);
        }
    }
}
