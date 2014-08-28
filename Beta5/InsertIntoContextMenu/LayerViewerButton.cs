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
using System.Linq;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Mapping;

namespace ProSDKSamples.InsertIntoContextMenu
{
    internal class LayerViewerButton : Button
    {
        // respond to click event
        protected override async void OnClick()
        {
            string xml = "";
            var toc = MappingModule.ActiveTOC;
            if (toc != null)
            {
                // get toc highlighted layers
                var selLayers = toc.SelectedLayers;
                // retrieve the first one
                Layer layer = selLayers.FirstOrDefault();
                if (layer != null)
                {
                    // find the CIM and serialize it
                    CIMBaseLayer cim = await layer.QueryLayerDefinitionAsync();
                    xml = XmlUtil.SerializeCartoXObject(cim);
                }
            }

            if (string.IsNullOrEmpty(xml))
                return;

            // show it
            CIMViewerViewModel vm = new CIMViewerViewModel();
            vm.Xml = xml;

            ArcGIS.Desktop.Internal.Framework.DialogManager.ShowDialog(vm, null);
        }
    }
}
