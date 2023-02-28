using System.Collections;
using System.Collections.Generic;
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

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI timeOffStartText;
        public TextMeshProUGUI hoursText;
        public TextMeshProUGUI dateTimeRequestedText;
        public TextMeshProUGUI statusText;
        
        private CultureInfo _culture = new CultureInfo("en-US");

        private void SetNewRequest(TimeOffRequest request)
        {
            nameText.text = request.EmployeeName;
            titleText.text = Regex.Replace(request.JobTitle.ToString(), "([A-Z])", " $1").Trim();
            timeOffStartText.text = request.TimeOffStart.ToString(_culture);
            hoursText.text = request.Hours.ToString(_culture);
            dateTimeRequestedText.text = request.RequestedOn.ToString(_culture);
            statusText.text = request.Status.ToString();
        }
    }
}
