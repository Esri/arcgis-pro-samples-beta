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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Internal.Mapping;
using ArcGIS.Desktop.Framework;

namespace UCSamples.CameraHeading
{
    /// <summary>
    /// View model for manipulating the camera heading based on the custom user control
    /// </summary>
    class HeadingViewModel : CustomControl
    {
        private double _headingValue;

        public HeadingViewModel()
        {

            MapView activeMapView = ForTheUcModule.ActiveMapView;
            _headingValue = activeMapView.Camera.Heading;

            FrameworkApplication.EventAggregator.GetEvent<ViewerExtentChanged>().Subscribe(CameraChanged);
        }

        ~HeadingViewModel()
        {
            FrameworkApplication.EventAggregator.GetEvent<ViewerExtentChanged>().Unsubscribe(CameraChanged);
        }

        public double CurrentHeadingValue
        {
            get 
            {
                return _headingValue;
                
            }
            set 
            {
                double cameraHeading = value > 180 ? value - 360 : value;

                MapView activeMapView = ForTheUcModule.ActiveMapView;

                Camera camera = ForTheUcModule.ActiveMapView.Camera;
                camera.Heading = cameraHeading;

                activeMapView.Camera = camera;

                _headingValue = value;
            }
        }

        private void CameraChanged(ViewEventArgs e)
        {
            double viewHeading = e.View.AutomationCamera.Heading < 0 ? 360 + e.View.AutomationCamera.Heading : e.View.AutomationCamera.Heading;

            SetProperty(ref _headingValue, viewHeading, () => CurrentHeadingValue);
        }
    }
}
