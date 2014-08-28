using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Internal.Mapping;

namespace UCSamples.ProjectItemProjectContainer
{
    internal class DeleteMap : Button
    {
        protected override async void OnClick()
        {
            // get the map container
            var container = ProjectModule.CurrentProject.GetProjectItemContainer<MapContainer>("Map");
            if (container == null)
                return;

            // get the first item
            MapProjectItem item = container.GetProjectItems().FirstOrDefault();
            if (item == null)
                return;

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
            await ProjectModule.CurrentProject.RemoveProjectItemAsync("Map", item.Path);
        }
    }
}
