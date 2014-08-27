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
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;

namespace UCSamples.DockPaneDemo {
    public static class DockpaneUtils {

        /// <summary>
        /// ID of the dockpane
        /// </summary>
        public const string DockPaneID = "UCSamples_Dockpane_id";

        /// <summary>
        /// Shows the dock pane
        /// </summary>
        /// <param name="damlID">Dockpane ID</param>
        public static async void ShowDockPane(string damlID = DockPaneID) {
            var pane = FindDockPane(damlID);
            if (pane == null)
                return;
            if (!pane.IsVisible) {
                pane.Activate();

            }
            if (!((DockpaneViewModel)pane).HasBookmarksLoaded) {
                await ((DockpaneViewModel)pane).LoadBookmarks();
            }
        }
        /// <summary>
        /// Finds the dockpane using the dockpane ID
        /// </summary>
        /// <param name="damlID">Dockpane ID</param>
        /// <param name="createIfNeeded"></param>
        /// <returns></returns>
        public static DockPane FindDockPane(string damlID = DockPaneID, bool createIfNeeded = true) {
            if (!createIfNeeded && !FrameworkApplication.IsDockPaneCreated(damlID))
                return null;
            return FrameworkApplication.FindDockPane(damlID);
        }


    }
}
