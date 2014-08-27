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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Events;

namespace UCSamples.SubscribeEvents {

    public class CustomEventArgs : EventArgs {
        public string Tag { get; set; }
    }

    public class CustomEventChanged : CompositePresentationEvent<CustomEventArgs> {
      public static SubscriptionToken Subscribe(Action<CustomEventArgs> action, bool keepSubscriberAlive = false)
      {
        return FrameworkApplication.EventAggregator.GetEvent<CustomEventChanged>().Register(action, keepSubscriberAlive);
      }

      public static void Unsubscribe(Action<CustomEventArgs> action)
      {
        FrameworkApplication.EventAggregator.GetEvent<CustomEventChanged>().Unregister(action);
      }

      public static void Unsubscribe(SubscriptionToken token)
      {
        FrameworkApplication.EventAggregator.GetEvent<CustomEventChanged>().Unregister(token);
      }

      internal static void Publish(string tag) 
      {
        FrameworkApplication.EventAggregator.GetEvent<CustomEventChanged>().Broadcast(new CustomEventArgs{
              Tag = tag
              });
      }
    }

    internal class PublishEventButton : Button {

        private bool _initialized = false;

        protected override void OnClick() {
            if (!_initialized) {
                _initialized = true;
                ForTheUcModule.SubscribeToEvent(evt => {
                    MessageBox.Show(evt.Tag, "Event Received");
                });
            }
            string time = DateTime.Now.ToString("G");
            CustomEventChanged.Publish(string.Format("From {0} : {1}",
                this.GetType().ToString(), time));
        }
    }
}
