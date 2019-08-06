using System;
using System.Collections.Generic;
using System.Text;

namespace AzureDevopsServicesReports
{
    public class Constants
    {
        public static string ProjectName = "MyFirst"; //projectname
        public static string TeamName = "MyFirst Team";// Team name
        public static string ApiBaseUrl = "https://dev.azure.com/{YourOrgName}/";
        public static string WorkItemIdsUrl
        {
            get
            {
                return string.Concat(ApiBaseUrl, ProjectName, "/", TeamName, "/_apis/work/backlogs/Microsoft.TaskCategory/workItems?api-version=5.0-preview.1");
            }
        }
        public static string WorkItemDetailsUrl
        {
            get
            {
                return string.Concat(ApiBaseUrl, ProjectName, "/", "_apis/wit/workitems?ids={0}&api-version=5.0"); //{0}CSV of ids
            }
        }

        public static string Personalaccesstoken = "{Generated from Azure Devops portal}";
    }
}
