using UnityEngine;
using SFB;

namespace SchedulingUtilities
{
    public class OpenETMReport : MonoBehaviour
    {
        public TestGUI testGUI;
        public void OpenReport()
        {
            // var path = StandaloneFileBrowser.OpenFilePanel("Open ETM Report", "", "csv", false);
            var extensions = new[] {new ExtensionFilter("CSV Files", "csv")};
            StandaloneFileBrowser.OpenFilePanelAsync("Open ETM Report", "", extensions, false, strings =>
            {
                if (strings == null || strings.Length == 0) return;
                testGUI.CreateNewReportAndTable(strings[0]);
            });
        }
    }
}
