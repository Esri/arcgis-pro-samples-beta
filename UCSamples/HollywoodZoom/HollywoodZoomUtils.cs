﻿//Copyright 2014 Esri

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

namespace UCSamples.HollywoodZoom {
    internal class HollywoodZoomUtils {

        public static double StepHeading(double currentHeading, double stepSize) {
            double heading = currentHeading + stepSize;

            // ensure that we are in the <360 range
            heading = heading > 360 ? heading % 360 : heading;

            // transform the 0-360 range into the heading property ranging from -180 - 180
            heading = heading > 180 ? heading - 360 : heading;

            return heading;
        }
    }
}
