#region #Copyright

// ----------------------------------------------------------------------------------
//   COPYRIGHT (c) 2024 CONTOU CONSULTING
//   ALL RIGHTS RESERVED
//   AUTHOR: Kyle Vanderstoep
//   CREATED DATE: 2024/1/14
// ----------------------------------------------------------------------------------

#endregion

using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using LS.AcumaticaCustomizationAPI.Workers;
using Module = Autofac.Module;

namespace LS.AcumaticaCustomizationAPI
{
    public class ServiceRegistration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RunOnApplicationStart(MapRoutes);
            builder.RegisterType<CustomizationWorker>();
        }

        internal static void MapRoutes()
        {
            RouteTable.Routes.MapRoute("Customization", "lscustomization/{action}",
                new
                {
                    controller = "LSCustomization",
                    action     = "Save"
                }, new string[1] { typeof(LSCustomizationController).Namespace });
        }
    }
}