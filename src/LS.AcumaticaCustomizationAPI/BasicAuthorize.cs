#region #Copyright

// ----------------------------------------------------------------------------------
//   COPYRIGHT (c) 2024 CONTOU CONSULTING
//   ALL RIGHTS RESERVED
//   AUTHOR: Kyle Vanderstoep
//   CREATED DATE: 2024/1/14
// ----------------------------------------------------------------------------------

#endregion

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CommonServiceLocator;
using PX.Data;

namespace LS.AcumaticaCustomizationAPI
{
    public class BasicAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            bool authorized        = false,
                allowAnonymous     = false,
                missingCredentials = false;
            var anonActionAttributes =
                actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(true);
            var anonControllerAttributes = actionContext.ActionDescriptor.ControllerDescriptor
                                                        .GetCustomAttributes<AllowAnonymousAttribute>(true);

            if (anonActionAttributes.Count > 0 || anonControllerAttributes.Count > 0)
                allowAnonymous = true;
            AuthenticationHeaderValue authorizeHeader = actionContext.Request.Headers.Authorization;
            if (authorizeHeader != null && authorizeHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrEmpty(authorizeHeader.Parameter))
            {
                Encoding encoding    = Encoding.GetEncoding("ISO-8859-1");
                string   credintials = encoding.GetString(Convert.FromBase64String(authorizeHeader.Parameter));

                string[] splitted = credintials.Split(':');
                if (splitted.Length < 2)
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
                else
                {
                    var company  = string.Empty;
                    var username = string.Empty;
                    var password = string.Empty;
                    if (splitted.Length > 2)
                    {
                        company  = splitted[0].ToUpper();
                        username = splitted[1].ToUpper();
                        password = splitted[2];
                    }
                    else if (splitted.Length == 2)
                    {
                        username = splitted[0].ToUpper();
                        password = splitted[1];
                    }

                    if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                    {
                        IPXLogin login = ServiceLocator.Current.GetInstance(typeof(IPXLogin)) as IPXLogin;

                        bool loggedIn            = login.LoginUser(ref username, password);
                        if (loggedIn) authorized = true;
                    }
                    else
                    {
                        missingCredentials = true;
                    }
                }
            }
            else
            {
                missingCredentials = true;
            }

            if (!allowAnonymous && missingCredentials)
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            else if (!allowAnonymous && !authorized)
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
    }
}