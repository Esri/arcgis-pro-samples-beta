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
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;

namespace ProSDKSamples.FrameworkState
{
    /// <summary>
    /// Toggle the application state. Activating or Deactivating a state called
    /// <b>example_state</b>. State is referred to in the DAML via a condition named <b>example_state_condition</b>.
    /// The condition is set <b>True</b> by the Framework when its underlying state is activated and
    /// <b>False</b> when its underlying state is deactivated. Multiple states can be combined into a
    /// condition using AND, OR, XOR combinations.
    /// </summary>
    internal class ToggleStateButton : Button
    {
        public const string MyStateID = "example_state";

        protected override void OnClick()
        {
            ProSDKSampleModule.ToggleState(MyStateID);
        }
    }
}
