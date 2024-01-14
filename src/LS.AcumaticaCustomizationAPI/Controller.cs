#region #Copyright

// ----------------------------------------------------------------------------------
//   COPYRIGHT (c) 2024 CONTOU CONSULTING
//   ALL RIGHTS RESERVED
//   AUTHOR: Kyle Vanderstoep
//   CREATED DATE: 2024/1/14
// ----------------------------------------------------------------------------------

#endregion

using System.Web.Http;
using LS.AcumaticaCustomizationAPI.Workers;

namespace LS.AcumaticaCustomizationAPI
{
    public class LSCustomizationController : ApiController
    {
        private readonly CustomizationWorker _cstWorker;

        public LSCustomizationController(CustomizationWorker cstWorker)
        {
            _cstWorker = cstWorker;
        }

        [System.Web.Mvc.HttpPost]
        public IHttpActionResult Save(string project, string path)
        {
            _cstWorker.SaveProject(project, path);
            return Ok();
        }
    }
}