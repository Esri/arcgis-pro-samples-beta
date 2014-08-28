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
using ArcGIS.Desktop.Internal.Mapping;

namespace ProSDKSamples.HollywoodZoom
{
    /// <summary>
    /// The button shows a more 'dramatic' zoom-in functionality by manipulating multiple
    /// properties on the camera.
    /// In 2D view mode we are changing the scale and heading properties whereas in 3D the we the z-value,
    /// the heading and pitch to emulate a spiral towards earth.
    /// </summary>
    internal class HollywoodZoomIn : Button
    {
        private MapView activeMapView = null;
        private int _zoomSteps = 20;

        public HollywoodZoomIn()
        {
        }

        protected override void OnClick()
        {
            activeMapView = ProSDKSampleModule.ActiveMapView;
            Camera activeCamera = activeMapView.Camera;

            if (activeMapView.Is2D)
            {
                // in 2D we are changing the scale
                double scaleStep = activeCamera.Scale / _zoomSteps;
                activeCamera.Scale = activeCamera.Scale - scaleStep;
            }
            else
            {
                // in 3D we are changing the Z-value and the pitch (for drama)
                double heightZStep = activeCamera.EyeXYZ.Z / _zoomSteps;
                double pitchStep = 90.0 / _zoomSteps;

                activeCamera.Pitch = activeCamera.Pitch - pitchStep;
                activeCamera.EyeXYZ.Z = activeCamera.EyeXYZ.Z - heightZStep;
            }

            // the heading changes the same in 2D and 3D
            activeCamera.Heading = HollywoodZoomUtils.StepHeading(activeCamera.Heading, 30);

            // assign the changed camera back to the view
            activeMapView.Camera = activeCamera;

        }
    }
}
