using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevopsServicesReports
{
    public class ReportBuilder
    {
        public async Task<string> BuildReportHTML(string templatePath)
        {
            AzureDevopsAPIHelper apiHelper = new AzureDevopsAPIHelper();
            string ids = await apiHelper.GetWorkItemIds();
            List<ReportItemDetails> reportData = await apiHelper.GetWorkItemsDetails(ids);
            string htmlcontent = await BuildHtmlTable(reportData);
            string data = File.ReadAllText(templatePath + "\\template.html");
            return data.Replace("[TablePlaceholder]", htmlcontent);
        }

        private async Task<string> BuildHtmlTable(List<ReportItemDetails> reportData)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("<table width='100%' cellpadding='2'><th align='left'>Work item id</th><th align='left'>Title</th><th>State</th>");

            foreach (ReportItemDetails item in reportData)
            {
                sb.Append("<tr>");
                sb.Append("<td>");
                sb.Append(item.ID);
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(item.Title);
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(item.State);
                sb.Append("</td>");

                sb.Append("</tr>");
            }
            // sb.Append("</table>");

            return sb.ToString();
        }

        private async Task<string> BuildHtmlList(List<ReportItemDetails> reportData)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("<table width='100%' cellpadding='2'><th align='left'>Work item id</th><th align='left'>Title</th><th>State</th>");

            foreach (ReportItemDetails item in reportData)
            {
                sb.Append("<tr>");
                sb.Append("<td>");
                sb.Append(item.ID);
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(item.Title);
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(item.State);
                sb.Append("</td>");

                sb.Append("</tr>");
            }
            // sb.Append("</table>");

            return sb.ToString();
        }
    }
}
