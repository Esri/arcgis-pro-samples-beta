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
using System.Threading.Tasks;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Mapping;
using Geometry = ArcGIS.Core.Geometry.Geometry;

namespace ProSDKSamples.EditingCutFeatures {

    internal static class ExtendsEditing {
        /// <summary>
        /// Use to provide an enumeration wrapper for an item of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<T> Yield<T>(this T item) {
            yield return item;
        }

        /// <summary>
        /// Returns the feature class associated with layer.
        /// </summary>
        /// <param name="layer">The input layer.</param>
        /// <returns>The table or the feature class associated with the layer.</returns>
        public static async Task<Table> getFeatureClass(this Layer layer) {
            // get the feature class associated with the layer
            return await ArcGIS.Desktop.Internal.Editing.EditingModuleInternal.GetTableAsync(layer);
        }

        /// <summary>
        /// Performs a spatial query against the table/feature class.
        /// </summary>
        /// <remarks>It is assumed that the feature class and the search geometry are using the same spatial reference.</remarks>
        /// <param name="searchTable">The table/feature class to be searched.</param>
        /// <param name="searchGeometry">The geometry used to perform the spatial query.</param>
        /// <param name="spatialRelationship">The spatial relationship used by the spatial filter.</param>
        /// <returns></returns>
        public static RowCursor Search(this Table searchTable, Geometry searchGeometry, SpatialRelationship spatialRelationship) {
            // define a spatial query filter
            ArcGIS.Core.Data.SpatialQueryFilter spatialQueryFilter = new ArcGIS.Core.Data.SpatialQueryFilter();
            // pasing the search geometry to the spatial filter
            spatialQueryFilter.FilterGeometry = searchGeometry;
            // define the spatial relationship between search geometry and feature class
            spatialQueryFilter.SpatialRelationship = spatialRelationship;

            // apply the spatial filter to the feature class in question
            return searchTable.Search(spatialQueryFilter);
        }

        
    }
}
