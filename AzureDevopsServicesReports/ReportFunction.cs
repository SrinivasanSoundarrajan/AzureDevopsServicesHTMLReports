using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using IronPdf;
using Microsoft.Azure.WebJobs.Host;

namespace AzureDevopsServicesReports
{
    public static class ReportFunction
    {
        [FunctionName("Report")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log, Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string path = context.FunctionAppDirectory;

            ReportBuilder rbuilder = new ReportBuilder();

            string data = await rbuilder.BuildReportHTML(path);
            byte[] pdfBytes = await BuildPdf(data);

            var response = BuildResponse(pdfBytes);

            return response;
        }


        private static async Task<byte[]> BuildPdf(string html)
        {
            try
            {
                var Renderer = new IronPdf.HtmlToPdf();


                Renderer.PrintOptions.Footer = new HtmlHeaderFooter()
                {
                    Height = 15,
                    HtmlFragment = "<center><i>{page} of {total-pages}<i></center>",
                    DrawDividerLine = true
                };


                //Image CDN url 
                Renderer.PrintOptions.Header = new HtmlHeaderFooter() { HtmlFragment = "<img alt='Link broken' src='" + "https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_92x30dp.png" + "' />" };


                IronPdf.PdfDocument PDF = Renderer.RenderHtmlAsPdf(html);
                return PDF.BinaryData;

                //Save to local
                //var OutputPath = "HtmlToPDF.pdf";
                //PDF.SaveAs(OutputPath);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static HttpResponseMessage BuildResponse(byte[] pdfBytes)
        {
            //return new FileContentResult(pdfBytes, "application/pdf");

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            //Set the Word document saved stream as content of response
            response.Content = new ByteArrayContent(pdfBytes);
            //Set the contentDisposition as attachment
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "report.Pdf"
            };
            //Set the content type as Word document mime type
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            //Return the response with output Word document stream
            return response;


        }
    }
}
