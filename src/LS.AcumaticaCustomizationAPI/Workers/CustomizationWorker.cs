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
using PX.SM;
using PX.Web.Customization;

namespace LS.AcumaticaCustomizationAPI.Workers
{
    public class CustomizationWorker
    {
        public void SaveProject(string projectName, string path)
        {
            ProjMaintOutput output =
                InitProjectMaintenanceGraphAndSetConfig(projectName, path);
            output.SourceControl.GetMethod("SaveProject").Invoke(null, new[] { projectName, output.CstDocument });
        }

        public void ReloadFromDatabase(string projectName)
        {
            ProjectBrowserMaint.SelectProjectByName(projectName);
            ProjectScriptMaintenance projectScriptMaintenence = new();
            projectScriptMaintenence.ActionUpdateFromDatabase.Press();
        }

        private static Type GetPXSourceControlType()
        {
            return Type.GetType("PX.SM.PXSourceControl, PX.Web.Customization");
        }

        private static ProjMaintOutput InitProjectMaintenanceGraphAndSetConfig(string projectName, string path)
        {
            ProjectMaintenance graph = new();
            graph.Projects.Current = graph.Projects.Search<CustProject.name>(projectName);
            Type      pxSourceControl       = GetPXSourceControlType();
            FieldInfo defaultFolderProperty = pxSourceControl.GetField("DefaultFolder");
            defaultFolderProperty.SetValue(null,
                Directory.Exists(path)
                    ? path
                    : throw new DirectoryNotFoundException("The directory is not found: " + path));
            var bindingsArray = pxSourceControl.GetField("Bindings").GetValue(null) as Dictionary<string, string>;
            bindingsArray[projectName] = path;
            pxSourceControl.GetMethod("SaveConfig").Invoke(null, new object[0]);
            object cstDocument = graph.GetType()
                                      .GetProperty("CstDocument", BindingFlags.NonPublic | BindingFlags.Instance)
                                      .GetValue(graph);
            return new ProjMaintOutput
            {
                SourceControl = pxSourceControl,
                CstDocument   = cstDocument
            };
        }

        private class ProjMaintOutput
        {
            public Type   SourceControl { get; set; }
            public object CstDocument   { get; set; }
        }
    }
}