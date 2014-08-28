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

namespace ProSDKSamples.FixedZoom
{
    /// <summary>
    /// The button shows zoom-in functionality by manipulating the camera.
    /// In 2D view mode we are changing the scale property whereas in 3D the z-value
    /// of the observer (eye).
    /// </summary>
    internal class FixedZoomIn : Button {

        private static readonly double MinimumScale = 10000.0;
        private static readonly double MinimumElevation = 1000;

        protected override void OnClick() {
            MapView activeMapView = ProSDKSampleModule.ActiveMapView;
            Camera camera = activeMapView.Camera;

            if (activeMapView.Is2D) {
                double scale = camera.Scale * 0.75;
                camera.Scale = scale > MinimumScale ? scale : MinimumScale;
            }
            else {
                double z = camera.EyeXYZ.Z * 0.75;
                camera.EyeXYZ.Z = z > MinimumElevation ? z : MinimumElevation;
            }
            activeMapView.Camera = camera;
        }
    }
}
