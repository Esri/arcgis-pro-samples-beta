﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Core;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Internal.Mapping;

namespace UCSamples.ProjectItemProjectContainer
{
    internal class OpenMap : Button
    {
        protected override void OnClick()
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
