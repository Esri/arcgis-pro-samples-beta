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
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.EditTools;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Framework;

namespace ProSDKSamples.EditingCutFeatures
{
    internal class CutFeaturesTool : SketchTool
    {
        /// <summary>
        /// Cutting tool that operates all features that are 'crossed' be the sketched line
        /// </summary>
        public CutFeaturesTool()
        {
            // select the type of construction tool you wish to implement.  
            // Make sure that the tool is correctly registered with the correct component category type in the daml
            //SketchType = SketchGeometryType.Point;
            SketchType = SketchGeometryType.Line;
            // SketchType = SketchGeometryType.Polygon;
            
        }

        /// <summary>
        /// We are being activated. Our tool has been selected on the UI
        /// </summary>
        /// <returns>Task</returns>
        protected override Task OnActivateImpl() {
            FrameworkApplication.State.Activate("esri_editing_CreatingFeature");
            return base.StartSketch(this.CurrentTemplate);
        }

        /// <summary>
        /// We are being deactivated (typically because a different tool has been selected)
        /// </summary>
        /// <returns>Task</returns>
        protected override Task OnDeactivateImpl() {
            FrameworkApplication.State.Deactivate("esri_editing_CreatingFeature");
            return Task.FromResult<int>(0);
        }

        protected override Task OnReactivateImpl() {
          return OnSketchSymbolNeedsUpdate();
        }

        /// <summary>
        /// This is a basic FinishSketch method which illustrates the process of using the sketch geometry for a cut. 
        /// 1. look at all layers in the active map
        /// for each layer:
        ///   2. Use the sketch geometry to perform a spatial query (Crosses)
        ///   3. Use the found features and use them to set up a cut operation
        ///   4. Create edit operation       
        ///   5. Execute the edit operation
        ///      ! edits are not saved !
        ///  
        /// </summary>
        /// <returns>Task of bool</returns>
        protected override async Task<bool> OnFinishSketch(Geometry geometry, Dictionary<string, object> attributes)
        {
            // use all layers of the active map to perform the cut
            foreach (var layer in ActiveMap.Layers)
            {
                // intialize a list of ObjectIDs that need to be cut
                List<long> cutOiDs = new List<long>();

                Table fc = await layer.getFeatureClass();
                // on a separate thread
                await QueuingTaskFactory.StartNew(() =>
                {
                    // find the features crossed by the sketch geometry
                    RowCursor rc = fc.Search(geometry, SpatialRelationship.Crosses);
                    // add the feature IDs into our prepared list
                    while (rc.MoveNext())
                    {
                        cutOiDs.Add(rc.Current.ObjectID);
                    }
                });
                // no features 'crossed' by the sketched line
                if (!cutOiDs.Any()) continue;

                // create an edit operation for the cut
                EditOperation op = EditingModule.CreateEditOperation();
                op.Name = string.Format("Cut {0}", layer.Name);
                op.ProgressMessage = "Working...";
                op.CancelMessage = "Operation canceled";
                op.ErrorMessage = "Error cutting features";
                op.SelectModifiedFeatures = false;
                op.SelectNewFeatures = false;

                // for each of the found features set up a cut method inside our edit operation
                // for multiple ObjectIDs the cuts with will be stacked into one operation
                foreach (var oid in cutOiDs)
                {
                    op.Cut(layer, oid, geometry);
                }
                await op.ExecuteAsync();
            }

            //execute the operation
            return true;
        }

    }
}
