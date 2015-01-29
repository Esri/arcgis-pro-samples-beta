using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Internal.Core;

namespace ProSDKSamples.ProjectItemProjectContainer
{
  /// <summary>
  /// Button for adding a new project item to the project pane.
  /// </summary>
  class AddNewProjectItem : Button
  {
    protected override async void OnClick()
    {
      int idx = 1;

      // use idx to ensure that the projectItems path is unique
      IEnumerable<SampleProjectItem> items = ProjectModule.CurrentProject.GetSampleItems();
      if (items != null)
      {
        foreach (SampleProjectItem item in items)
          idx++;
      }

      // add it to the project
      await (ProjectModule.CurrentProject as IInternalGISProjectItem).AddProjectItemAsync("SampleProjectItems", "projectItem" + idx.ToString(), "ProjectItem " + idx.ToString(), "");
    }
  }
}
