using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Internal.Core;
using ArcGIS.Desktop.Internal.Mapping;
using ArcGIS.Desktop.Mapping;

namespace ProSDKSamples.ProjectItemProjectContainer
{
    /// <summary>
    ///  Sample button illustrating how to access the map project items.  In this scenario we attempt to delete the first map from the project.
    /// </summary>
    internal class DeleteMap : Button
    {
        protected override async void OnClick()
        {
            // find the first map item
            MapProjectItem item = ProjectModule.CurrentProject.GetMaps().FirstOrDefault();
            if (item == null)
                return;

            // alternate example - find the map item using Name
            //IEnumerable<MapProjectItem> maps = ProjectModule.CurrentProject.GetMaps();
            //item = ProjectModule.CurrentProject.GetMaps().FirstOrDefault(m => m.Name == "Layers");

            // fimd the map
            Map map = MappingModule.FindMap(item.Path);
            if (map == null)
                return;

            // close if already open
            IList<IMapPane> mapPanes = MappingModule.GetMapPanes(map);
            if ((mapPanes != null) && (mapPanes.Count > 0))
            {
                for (int idx = mapPanes.Count-1; idx >= 0; idx--)
                {
                    Pane pane = mapPanes[idx] as Pane;
                    pane.Close();
                }
            }

            // remove it from project
            await (ProjectModule.CurrentProject as IInternalGISProjectItem).RemoveProjectItemAsync("Map", item.Path);
        }
    }
}
