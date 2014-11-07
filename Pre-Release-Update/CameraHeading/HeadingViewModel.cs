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
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Events;

namespace ProSDKSamples.CameraHeading
{
    /// <summary>
    /// View model for manipulating the camera heading based on the custom user control
    /// </summary>
    class HeadingViewModel : CustomControl
    {
        public HeadingViewModel()
        {
            CameraChangedEvents.Subscribe(CameraChanged);
            ActiveMapViewChangedEvents.Subscribe(OnActiveViewChanged);
            InitalizeAsync();
        }

        ~HeadingViewModel()
        {
          CameraChangedEvents.Unsubscribe(CameraChanged);
          ActiveMapViewChangedEvents.Unsubscribe(OnActiveViewChanged);
        }

        private async Task InitalizeAsync()
        {
          MapView activeMapView = ProSDKSampleModule.ActiveMapView;
          if ((activeMapView != null))
          {
            Camera = await activeMapView.GetCameraAsync();
            _headingValue = Camera.Heading;
            _enableCamera = true;
          }
          else
          {
            _headingValue = 0;
            _enableCamera = false;
          }
        }

        private bool _enableCamera = false;
        public bool IsCameraEnabled 
        {
            get
            {
                return _enableCamera;
            }
            set
            {
                SetProperty(ref _enableCamera, value, () => IsCameraEnabled);
            }
        }

        private double _headingValue;
        public double HeadingValue
        {
          get
          {
            return _headingValue;

          }
          set
          {
            double cameraHeading = value > 180 ? value - 360 : value;
            _headingValue = value;
            Camera.Heading = cameraHeading;

            MapView activeMapView = ProSDKSampleModule.ActiveMapView;
            if (activeMapView != null)            
              activeMapView.ZoomToAsync(Camera);            
          }
        }

        private Camera Camera{ get; set; }

        private void CameraChanged(CameraEventArgs args)
        {
          Camera = args.CurrentCamera;
          SetHeading();
        }

        private async void OnActiveViewChanged(MapViewEventArgs args)
        {
          MapView mapView = args.MapView;
          if (mapView != null)
          {
            IsCameraEnabled = true;
            Camera = await mapView.GetCameraAsync();
            SetHeading();
          }
          else
            IsCameraEnabled = false;
        }

        private void SetHeading()
        {
          if (Camera != null)
          {
            double viewHeading = Camera.Heading < 0 ? 360 + Camera.Heading : Camera.Heading;
            SetProperty(ref _headingValue, viewHeading, () => HeadingValue);
          }
        }
    }
}
