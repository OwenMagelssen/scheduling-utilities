using System;
using System.Collections.Generic;
using System.IO;
using SFB;

namespace SchedulingUtilities
{
    public static class GenerateEmail
    {
        public static void FromTimeOffRequestReport(TimeOffRequestReport report)
        {
            string emailContents = string.Empty;
            emailContents += "<!DOCTYPE html>\n";
            emailContents += "<html>\n";

            for (int i = 0; i < report.employeesWithRequests.Count; i++)
            {
                var employeeName = report.employeesWithRequests[i];
                var employeeRequests = report.employeeRequestMap[report.employeesWithRequests[i]];
                emailContents += GetMailToLink(employeeName, employeeRequests);
            }
            
            emailContents += "</html>\n";
            
            StandaloneFileBrowser.SaveFilePanelAsync("Save email Link Page", "", "email_links", "html", path =>
            {
                File.WriteAllText(path, emailContents);
            });
        }

        //TODO: this will have to be a lookup table
        private static string GetEmployeeEmail(string employeeNameFirstLast)
        {
            string[] names = employeeNameFirstLast.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return $"{names[0]}.{names[1]}@seattlechildrens.org";
        }

        private static string GetMailToLink(string employeeName, List<TimeOffRequest> requests)
        {
            string email = GetEmployeeEmail(employeeName);
            
            string emailBody = "The following";
            
            var link = $"<a href=\"mailto:{email}?cc=SecurityScheduling@seattlechildrens.org&subject=PTO Requests&body={emailBody}\">Send Email to {employeeName}</a><br>";
            return link;
        }
    }
}