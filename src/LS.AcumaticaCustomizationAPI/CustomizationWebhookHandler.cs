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

            if (method.Count < 1)
                WriteResponse(context, "Please provide a method parameter", 400);
            if (projectName.Count < 1)
                WriteResponse(context, "Please provide a projectName parameter", 400);
            else
                switch (method[0]?.ToLower())
                {
                    case "reloadfromdb":
                        CustomizationWorker.ReloadFromDatabase(projectName[0]);
                        break;
                    case "saveproject":
                        CustomizationWorker.SaveProject(projectName[0],
                            "D:\\Repos\\acumatica-customizationAPI\\CustomizationAPI_23_1\\test");
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
            context.Response.StatusCode = statusCode;
        }
    }
}