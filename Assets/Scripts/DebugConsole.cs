using UnityEngine;
using TMPro;

namespace SchedulingUtilities
{
    public class DebugConsole : MonoBehaviour
    {
        public TextMeshProUGUI text;

        private void Awake()
        {
            Application.logMessageReceived += (logString, stackTrace, logType) =>
            {
                text.text += logString + "\n" + stackTrace + "\n\n";
            };
        }
    }
}
