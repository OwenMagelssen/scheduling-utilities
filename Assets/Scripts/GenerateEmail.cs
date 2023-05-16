using System.IO;
using SFB;

namespace SchedulingUtilities
{
    public static class GenerateEmail
    {
        public static void FromTimeOffRequest(TimeOffRequest request)
        {
            string emailContents = string.Empty;
            emailContents += "<!DOCTYPE html>\n";
            emailContents += "<html>\n";
            emailContents += "<body>\n";
            emailContents += $"test email contents.<br>test second line of email contents.<br>Name: {request.EmployeeName}\n";
            emailContents += "</body>\n";
            emailContents += "<a href=\"mailto:owen.magelssen@seattlechildrens.org\">Send Email</a>";
            emailContents += "</html>\n";
            
            // var extensions = new[] {new ExtensionFilter("CSV Files", "csv")};
            
            StandaloneFileBrowser.SaveFilePanelAsync("Save email Link Page", "", "email_links", "html", path =>
            {
                File.WriteAllText(path, emailContents);
            });
        }
    }
}