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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Mapping;

namespace UCSamples.Geocode {
    internal static class GeocodeUtils {

        private static Dictionary<int, CIMPointGraphicHelper> _lookup = new Dictionary<int, CIMPointGraphicHelper>();
        /// <summary>
        /// Do a search for the contents of the specified URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static CandidateResponse SearchFor(string text, int numResults = 2) {
            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", "GeocodeExample");
            wc.Headers.Add("referer", "GeocodeExample");
            wc.Encoding = System.Text.Encoding.UTF8;

            CandidateResponse geocodeResult = null;
            using (StreamReader sr = new StreamReader(wc.OpenRead(new UCSamples.Geocode.GeocodeURI(text, numResults).Uri),
                                                      System.Text.Encoding.UTF8, true)) {
                string response = sr.ReadToEnd();
                if (ResponseIsError(response)) {
                    //throw
                    throw new System.ApplicationException(response);
                }
                geocodeResult = ObjectSerialization.JsonToObject<CandidateResponse>(response);
            }
            return geocodeResult;
        }

        /// <summary>
        /// Check if the returned response is an error
        /// message from the AGS
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static bool ResponseIsError(string response) {
            return response.Substring(0, "{\"error\":".Length).CompareTo("{\"error\":") == 0;
        }

        #region Display
        /// <summary>
        /// Zoom to the specified location
        /// </summary>
        /// <param name="extent">The extent of the geocoded candidate</param>
        /// <returns></returns>
        public static Task ZoomToLocation(CandidateExtent extent) {

            return QueuingTaskFactory.StartNew(() => {
                ArcGIS.Core.Geometry.Envelope envelope = new ArcGIS.Core.Geometry.Envelope(
                     extent.XMin, extent.YMin, extent.XMax, extent.YMax,
                     new ArcGIS.Core.Geometry.SpatialReference(extent.WKID)
                     );
                //apply extent
                ForTheUcModule.ActiveMapView.ZoomTo(GeometryEngine.Expand(envelope, 3, 3, true));
            });
        }

        #endregion Display

        #region Graphics Support

        ///// <summary>
        ///// Make a CIMPointGraphic that can be added to the map overlay
        ///// </summary>
        ///// <param name="point">The location of the graphic</param>
        ///// <returns></returns>
        //internal static CIMPointGraphicHelper MakeCIMPointGraphic(PointN point)
        //{
        //    return new CIMPointGraphicHelper(point);
        //}
        /// <summary>
        /// Add a point to the specified mapview
        /// </summary>
        /// <param name="mapView">The mapview to whose overlay the graphic will be added</param>
        /// <returns>The graphic id assigned to the graphic in the overlay</returns>
        public static void AddToMapOverlay(ArcGIS.Core.CIM.PointN point, MapView mapView) {
            if (!mapView.Is2D)
                return;//only currently works for 2D

            CIMPointGraphicHelper graphicHlpr = new CIMPointGraphicHelper(point);
            graphicHlpr.graphicID = mapView.AddOverlayGraphic(graphicHlpr.XML);
            _lookup[mapView.Map.RepositoryID] = graphicHlpr;

        }

        /// <summary>
        /// All-in-one. Update the graphic on the overlay if it was previously added
        /// otherwise, make it and add it
        /// </summary>
        /// <param name="newLocation">The new location to be added to the map</param>
        /// <param name="mapView"></param>
        /// <returns></returns>
        public static void UpdateMapOverlay(ArcGIS.Core.CIM.PointN point, MapView mapView) {
            if (!mapView.Is2D)
                return;//only currently works for 2D

            //CIMPointGraphicHelper graphicHlpr = null;
            if (_lookup.ContainsKey(mapView.Map.RepositoryID)) {
                ////graphicHlpr = _lookup[mapView.Map.ID];
                ////graphicHlpr.UpdateLocation(point);
                ////int id = graphicHlpr.graphicID;
                ////mapView.UpdateOverlayGraphic(ref id, graphicHlpr.XML);
                ////graphicHlpr.graphicID = id;
                RemoveFromMapOverlay(mapView);
                AddToMapOverlay(point, mapView);
            }
            else {
                //first time
                AddToMapOverlay(point, mapView);
            }
        }
        /// <summary>
        /// Remove the Point Graphic from the specified mapview
        /// </summary>
        /// <param name="mapView"></param>
        public static void RemoveFromMapOverlay(MapView mapView) {
            if (!mapView.Is2D)
                return;//only currently works for 2D
            if (_lookup.ContainsKey(mapView.Map.RepositoryID)) {
                mapView.RemoveOverlayGraphic(_lookup[mapView.Map.RepositoryID].graphicID);
                CIMPointGraphicHelper graphicHlpr = _lookup[mapView.Map.RepositoryID];
                _lookup.Remove(mapView.Map.RepositoryID);
                graphicHlpr = null;

            }
        }

        #endregion Graphics Support
    }
}
