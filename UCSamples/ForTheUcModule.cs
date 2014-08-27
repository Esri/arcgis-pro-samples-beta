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
using System;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Events;
using ArcGIS.Desktop.Internal.Core;
using ArcGIS.Desktop.Internal.Mapping;
using UCSamples.GP;
using UCSamples.ProjectItemProjectContainer;
using UCSamples.SubscribeEvents;
using Module = ArcGIS.Desktop.Framework.Contracts.Module;

namespace UCSamples
{

    internal class ForTheUcModule : Module
    {
        private static ForTheUcModule _this = null;
        private GPHelper _gpHelper = null;

        /// <summary>
        /// Retrieve the singleton instance to this module here
        /// </summary>
        public static ForTheUcModule Current
        {
            get
            {
                return _this ?? (_this = (ForTheUcModule)FrameworkApplication.FindModule("UCSamples_Module1_id"));
            }
        }

        public static MapView ActiveMapView {
            get {
                var activePane = FrameworkApplication.Panes.ActivePane as ViewerPaneViewModel;
                if ((activePane == null) || !activePane.IsViewerPaneInitialized)
                    return null;
                return activePane.GetActiveView() as MapView;
            }
        }

        /// <summary>
        /// Gets the GP Helper
        /// </summary>
        public GPHelper GPHelper {
            get {
                if (_gpHelper == null)
                    _gpHelper = new GPHelper();
                return _gpHelper;
            }
        }

        #region Toggle State
        /// <summary>
        /// Activate or Deactivate the specified state. State is identified via
        /// its name. Listen for state changes via the DAML <b>condition</b> attribute
        /// </summary>
        /// <param name="stateID"></param>
        public static void ToggleState(string stateID) {
            if (FrameworkApplication.State.Contains(stateID)) {
                FrameworkApplication.State.Deactivate(stateID);
            }
            else {
                FrameworkApplication.State.Activate(stateID);
            }
        }

        #endregion Toggle State

        #region For the Custom Event

        public static SubscriptionToken SubscribeToEvent(Action<CustomEventArgs> action)
        {
          return CustomEventChanged.Subscribe(action);
        }

        public static void UnsubscribeToEvent(SubscriptionToken token)
        {
          CustomEventChanged.Unsubscribe(token);
        }

        #endregion For the Custom Event

        #region Project Container/Items Sample

        internal static async void AddContainerItem()
        {
          int idx = 1;

          // projectItemContainer key must match that of content type attribute as specified in config.xml 
          SampleProjectContainer container = ProjectModule.CurrentProject.GetProjectItemContainer("SampleProjectItems") as SampleProjectContainer;
          if (container != null)
          {
            foreach (SampleProjectItem item in container.GetProjectItems())
            {
              idx++;
            }
          }
          // add it to the project
          await (ProjectModule.CurrentProject as IInternalGISProjectItem).AddProjectItemAsync("SampleProjectItems", "projectItem " + idx.ToString(), "ProjectItem " + idx.ToString(), "");
        }
        #endregion
    }
}
