#region #Copyright

// ----------------------------------------------------------------------------------
//   COPYRIGHT (c) 2024 CONTOU CONSULTING
//   ALL RIGHTS RESERVED
//   AUTHOR: Kyle Vanderstoep
//   CREATED DATE: 2024/1/18
// ----------------------------------------------------------------------------------

#endregion

using Autofac;
using LS.AcumaticaCustomizationAPI.Workers;

namespace LS.AcumaticaCustomizationAPI.Autofac
{
    public class Module : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomizationWorker>();
        }
    }
}