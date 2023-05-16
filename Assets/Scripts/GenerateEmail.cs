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
                if (string.IsNullOrEmpty(path)) return;
                File.WriteAllText(path, emailContents);
            });
        }

        //TODO: this will have to be a lookup table
        private static string GetEmployeeEmail(string employeeNameFirstLast)
        {
            string[] names = employeeNameFirstLast.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return $"{names[0]}.{names[1]}@seattlechildrens.org";
        }

        private static string Break = "%0D%0A";

        private static string GetRequestDetails(TimeOffRequest request)
        {
            return $"{request.TimeOffStart.ToString("MM/dd/yyyy hh:mm tt")}    Hours: {request.Hours.ToString("0.0")}%0D%0A";
        }

        private static string GetMailToLink(string employeeName, List<TimeOffRequest> requests)
        {
            string email = GetEmployeeEmail(employeeName);
            
            string emailBody = "The following PTO requests have been approved:%0D%0A%0D%0A";

            for (int i = 0; i < requests.Count; i++)
            {
                if (requests[i].Status == Status.Approved)
                    emailBody += GetRequestDetails(requests[i]);
            }
            
            emailBody += "%0D%0A%0D%0AThe following PTO requests have been denied:%0D%0A";

            for (int i = 0; i < requests.Count; i++)
            {
                if (requests[i].Status == Status.Denied)
                    emailBody += GetRequestDetails(requests[i]);
            }
            
            emailBody += "%0D%0A%0D%0AThe following PTO requests are pending:%0D%0A";

            for (int i = 0; i < requests.Count; i++)
            {
                if (requests[i].Status == Status.Pending) 
                    emailBody += GetRequestDetails(requests[i]);
            }
            
            var link = $"<a href=\"mailto:{email}?cc=SecurityScheduling@seattlechildrens.org&subject=PTO Requests&body={emailBody}\">Send Email to {employeeName}</a><br>";
            return link;
        }
    }
}