using System.Globalization;
using UnityEngine;

namespace SchedulingUtilities
{
    public class TestGUI : MonoBehaviour
    {
        public TimeOffRequestReport report;

        public void OnGUI()
        {
            if (report == null) return;
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
    }
}
