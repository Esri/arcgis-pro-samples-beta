using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Mapping;

namespace ProSDKSamples.ProjectItemProjectContainer
{
    /// <summary>
    ///  Sample button illustrating how to access the map project items.  In this scenario we attempt to open the first map from the project.
    /// </summary>
    internal class OpenMap : Button
    {
      protected override void OnClick()
      {
        // get the first map item
        MapProjectItem item = ProjectModule.CurrentProject.GetMaps().FirstOrDefault();
        if (item == null)
          return;

        // fimd the map
        Map map = MappingModule.FindMap(item.Path);
        if (map == null)
          return;

        bool bAlreadyOpen = false;

        // see if its already open
        //IList<IMapPane> mapPanes = MappingModule.GetMapPanes(map);
        //if ((mapPanes != null) && (mapPanes.Count > 0))
        //    bAlreadyOpen = true;

        if (!bAlreadyOpen)
        {
          var mapPane = MappingModule.OpenMapView(item.Path, item.ViewingMode);
        }
      }

    }
}
