//Copyright 2014 Esri

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Internal.Mapping;
using ArcGIS.Desktop.Internal.Mapping.Table;
using ArcGIS.Desktop.Mapping;

namespace ProSDKSamples.SubscribeEvents
{
    class TrapSelectionChangedCheckbox : ArcGIS.Desktop.Framework.Contracts.CheckBox
    {
        public TrapSelectionChangedCheckbox() : base() 
        {
        }

        ~TrapSelectionChangedCheckbox()
        {
          // make sure unsubscribe occurs  
          MapSelectionChanged.Unsubscribe(OnMapSelectionChanged);
        }

        /// <summary>
        /// checkbox click event.  Subscribe or Unsubscribe to the MapSelectionChanged event according to the checkbox value
        /// </summary>
        protected override void OnClick()
        {
            if (IsChecked == true)
                MapSelectionChanged.Subscribe(OnMapSelectionChanged);
            else if (IsChecked == false)
            {
                MapSelectionChanged.Unsubscribe(OnMapSelectionChanged);
                ITablePane tbl = TableManager.ActiveTablePane;
                while (tbl != null)
                {
                    ((Pane)tbl).Close();
                    tbl = TableManager.ActiveTablePane;
                }
            }
        }

        private async void OnMapSelectionChanged(ViewEventArgs obj)
        {
            MapView view = obj.View as MapView;
            if (view == null)
                return;

            // retrieve the selection set
            var allSelectedFeatures = await SelectionSet.QuerySelection(view.Map);
            // loop through the layer, OID sets
            foreach (var layerOIDSetPair in allSelectedFeatures.GetSelection())
            {
                var layer = layerOIDSetPair.Item1;
                var oids = layerOIDSetPair.Item2;

                // open the table view showing only selected records
                if (layer != null)
                    TableManager.OpenTablePane(layer, TableViewMode.eSelectedRecords);
            }
        }
    }
}
