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
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Mapping;

namespace ProSDKSamples.HollywoodZoom
{
    /// <summary>
    /// The button shows a more 'dramatic' zoom-out functionality by manipulating multiple
    /// properties on the camera.
    /// In 2D view mode we are changing the scale and heading properties whereas in 3D the we the z-value,
    /// the heading and pitch to emulate a spiral away from earth.
    /// </summary>
    internal class HollywoodZoomOut : Button
    {
        private MapView activeMapView = null;
        private int _zoomSteps = 20;

        protected override async void OnClick()
        {
            activeMapView = ProSDKSampleModule.ActiveMapView;
            Camera camera = await activeMapView.GetCameraAsync() ;

            bool is2D = false ;
            try {
              is2D = activeMapView.ViewMode == ViewMode.Map;
            }
            catch(System.ApplicationException) {
                return ;
            }

            if (is2D)
            {
                // in 2D we are changing the scale
                double scaleStep = camera.Scale / _zoomSteps;
                camera.Scale = camera.Scale + scaleStep;
            }
            else
            {
                // in 3D we are changing the Z-value and the pitch (for drama)
                double heightZStep = camera.Z / _zoomSteps;
                double pitchStep = 90.0 / _zoomSteps;

                camera.Pitch = camera.Pitch + pitchStep;
                camera.Z = camera.Z + heightZStep;
            }

            // the heading changes the same in 2D and 3D
            camera.Heading = HollywoodZoomUtils.StepHeading(camera.Heading, -30);

            // assign the changed camera back to the view
            activeMapView.ZoomToAsync(camera);
        }
    }
}
