//Copyright 2014 Esri

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Mapping;
using ArcGIS.Desktop.Mapping;

namespace UCSamples.DockPaneDemo {
    public static class ExtendsBookmark {

        /// <summary>
        /// Zooms to a bookmark in a map view
        /// </summary>
        /// <param name="bookMark"> Mapping module Bookmark object</param>
        /// <param name="mapView"> A Map view</param>
        public static async void ZoomTo(this MapView mapView, Bookmark bookMark) {
            ArcGIS.Core.CIM.CIMBookmark bmkDef = await bookMark.QueryDefinition();
            mapView.Extent = bmkDef.Location;
        }
        /// <summary>
        /// Pans to a bookmark in a map view
        /// </summary>
        /// <param name="bookMark"> Mapping module Bookmark object</param>
        /// <param name="mapView"> A Map view</param>
        public static async void PanTo(this MapView mapView, Bookmark bookMark) {
            ArcGIS.Core.CIM.CIMBookmark bmkDef = await bookMark.QueryDefinition();
            mapView.PanTo(bmkDef.Location);
        }
        /// <summary>
        /// Obtains the name of the map the bookmark is in
        /// </summary>
        /// <param name="bm">Mapping module Bookmark object</param>
        /// <returns></returns>
        public static string GetMapName(this Bookmark bookMark) {
            string mapname = "";
            // Find the map name
            var mapContainer = (ProjectModule.CurrentProject != null) ? ProjectModule.CurrentProject.GetProjectItemContainer<MapContainer>("Map") : null;
            if (mapContainer != null) {
                var projItem = mapContainer.GetProjectItems().FirstOrDefault<IProjectItem>((i) => i.Path == bookMark.MapPath);
                if (projItem != null)
                    mapname = projItem.Name;
            }

            return mapname;
        }

        /// <summary>
        /// Loads all the bookmarks found in the project
        /// </summary>
        /// <param name="currentProject">Current project</param>
        /// <returns></returns>
        public static Task<IList<Bookmark>> LoadBookmarksAsync(this Project currentProject) {
            return QueuingTaskFactory.StartNew < IList<Bookmark>>(async () => {
                var mc = currentProject.ProjectItemContainers.OfType<MapContainer>().FirstOrDefault();
                List<Bookmark> bookMarks = new List<Bookmark>();
                foreach (var mapItem in mc.GetProjectItems()) {
                    var map = await MappingModule.GetMapAsync(mapItem.Path);
                    if (map != null) {
                        var bmks = await map.QueryBookmarks();
                        if (null != bmks && bmks.Count() > 0) {
                            foreach (var bmk in bmks)
                                bookMarks.Add(bmk);
                        }
                    }
                }
                return bookMarks;
            });
        }
    }
}
