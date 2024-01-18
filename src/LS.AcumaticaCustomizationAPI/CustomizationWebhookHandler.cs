#region #Copyright

// ----------------------------------------------------------------------------------
//   COPYRIGHT (c) 2024 CONTOU CONSULTING
//   ALL RIGHTS RESERVED
//   AUTHOR: Kyle Vanderstoep
//   CREATED DATE: 2024/1/17
// ----------------------------------------------------------------------------------

#endregion

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using LS.AcumaticaCustomizationAPI.Workers;
using Microsoft.Extensions.Primitives;
using PX.Api.Webhooks;
using PX.Data;

namespace LS.AcumaticaCustomizationAPI
{
    public class CustomizationWebhookHandler : PXGraph<CustomizationWebhookHandler>, IWebhookHandler
    {
        [InjectDependency]
        public CustomizationWorker CustomizationWorker { get; set; }

        public async Task HandleAsync(WebhookContext context, CancellationToken cancellation)
        {
            StringValues method      = context.Request.Query["method"];
            StringValues projectName = context.Request.Query["Project"];
            StringValues directory   = context.Request.Query["Dir"];

            if (string.IsNullOrWhiteSpace(method))
                WriteResponse(context, "Please provide a method", 400);
            if (string.IsNullOrWhiteSpace(projectName))
                WriteResponse(context, "Please provide a projectName", 400);
            else
                switch (method[0]?.ToLower())
                {
                    case "reloadfromdb":
                        CustomizationWorker.ReloadFromDatabase(projectName[0]);
                        break;
                    case "saveproject":
                        if (string.IsNullOrWhiteSpace(directory))
                            WriteResponse(context, "Please provide a dir", 400);
                        CustomizationWorker.SaveProject(projectName[0], HttpUtility.UrlDecode(directory));
                        break;
                    default:
                        WriteResponse(context, $"Method {method[0]} not a valid method", 400);
                        break;
                }
        }

        private void WriteResponse(WebhookContext context, string body, int statusCode)
        {
            TextWriter writer = context.Response.CreateTextWriter();
            writer.Write(body);
            writer.Flush();
            context.Response.StatusCode = statusCode;
        }
    }
}