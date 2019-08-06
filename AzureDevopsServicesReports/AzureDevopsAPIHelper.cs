using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevopsServicesReports
{
    public class AzureDevopsAPIHelper
    {
        public async Task<string> GetWorkItemIds()
        {
            StringBuilder ids = new StringBuilder();

            try
            {
                string response = await GetFromAzureDevopsAPI(Constants.WorkItemIdsUrl);
                if (!string.IsNullOrEmpty(response))
                {
                    JObject res = JObject.Parse(response);
                    foreach (JObject wt in res["workItems"])
                    {
                        ids.Append(wt["target"]["id"].Value<string>());
                        ids.Append(",");
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return ids.ToString().TrimEnd(',');
        }

        public async Task<List<ReportItemDetails>> GetWorkItemsDetails(string ids)
        {
            List<ReportItemDetails> reportData = new List<ReportItemDetails>();
            ReportItemDetails item;
            try
            {
                string url = string.Format(Constants.WorkItemDetailsUrl, ids);
                string workitems = await GetFromAzureDevopsAPI(url);

                JObject result = JObject.Parse(workitems);

                if (result["value"] != null)
                {
                    JArray arr = result["value"].Value<JArray>();
                    foreach (JObject obj in arr)
                    {
                        item = new ReportItemDetails();
                        item.ID = obj["id"].Value<string>();
                        JObject fieldsObj = obj["fields"].Value<JObject>();
                        item.Title = fieldsObj["System.Title"].Value<string>();
                        item.State = fieldsObj["System.State"] != null ? fieldsObj["System.State"].Value<string>() : string.Empty;
                        item.Description = fieldsObj["System.Description"] != null ? fieldsObj["System.Description"].Value<string>() : string.Empty;
                        reportData.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return reportData;
        }

        private Task<string> GetFromAzureDevopsAPI(string url)
        {
            try

            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "", Constants.Personalaccesstoken))));

                    using (HttpResponseMessage response = client.GetAsync(
                                url).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        return response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
