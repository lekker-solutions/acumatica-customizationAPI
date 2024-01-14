#region #Copyright

// ----------------------------------------------------------------------------------
//   COPYRIGHT (c) 2024 CONTOU CONSULTING
//   ALL RIGHTS RESERVED
//   AUTHOR: Kyle Vanderstoep
//   CREATED DATE: 2024/1/14
// ----------------------------------------------------------------------------------

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using PX.Data;
using PX.SM;

namespace LS.AcumaticaCustomizationAPI.Workers
{
    public class CustomizationWorker
    {
        public void SaveProject(string projectName, string path)
        {
            CustProject custDoc =
                PXSelect<CustProject, Where<CustProject.name, Equal<Required<CustProject.name>>>>.Select(new PXGraph(),
                    projectName);
            ProjectMaintenance graph = new ProjectMaintenance();
            graph.Projects.Current = custDoc;
            var pxSourceControl = Type.GetType("PX.SM.PXSourceControl, PX.Web.Customization");
            var defaultFolderProperty = pxSourceControl.GetField("DefaultFolder");
            defaultFolderProperty.SetValue(null, Directory.Exists(path) ? path : throw new DirectoryNotFoundException("The directory is not found: " + path));
            var bindingsArray = pxSourceControl.GetField("Bindings").GetValue(null) as Dictionary<string, string>;
            bindingsArray[projectName] = path;
            pxSourceControl.GetMethod("SaveConfig").Invoke(null, new object[0]);
            object cstDocument = graph.GetType()
                                      .GetProperty("CstDocument", BindingFlags.NonPublic | BindingFlags.Instance)
                                      .GetValue(graph);
            pxSourceControl.GetMethod("SaveProject").Invoke(null, new[] { projectName, cstDocument });
            //PXSourceControl.SaveConfig();
            //PXSourceControl.SaveProject(path, graph.CstDocument);
        }
    }
}