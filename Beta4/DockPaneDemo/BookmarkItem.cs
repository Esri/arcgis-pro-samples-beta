//Copyright 2014 Esri

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Internal.Mapping;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace UCSamples.DockPaneDemo
{
    /// <summary>
    /// Represent the bookmark item
    /// </summary>
    public class BookmarkItem : INotifyPropertyChanged
    {
        private Bookmark _bmk;
        double _population = 0;

        private string _mapname;
        /// <summary>
        /// Gets the thumbnail of the bookmark
        /// </summary>
        public ImageSource Image
        {
            get
            {
                return _bmk.Thumbnail;
            }
        }
        /// <summary>
        /// Constructor for the bookmark item
        /// </summary>
        /// <param name="bmk">Mapping module Bookmark objeck</param>
        public BookmarkItem(Bookmark bmk)
        {
            _bmk = bmk;
            _mapname = _bmk.GetMapName();
            _zoomToCmd = new RelayCommand(() => ZoomTo());
        }

        /// <summary>
        /// Gets the bookmark's name
        /// </summary>
        public string Name
        {
            get
            {
                return _bmk.Name;
            }
        }
        /// <summary>
        /// Gets and sets the map name of the bookmark
        /// </summary>
        public string MapName
        {
            get
            {
                return _mapname;
            }
            set
            {
                MapName = _mapname;
                OnPropertyChanged("MapName");
            }
        }

        private RelayCommand _zoomToCmd;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Represents the Zoom command
        /// </summary>
        public ICommand ZoomToCmd
        {
            get { return _zoomToCmd; }
        }
        /// <summary>
        /// Represents the Zoom method
        /// </summary>
        public void ZoomTo()
        {
            var activePane = FrameworkApplication.Panes.ActivePane as ViewerPaneViewModel;
            if ((activePane == null) || !activePane.IsViewerPaneInitialized)
                return;
            MapView activeView = activePane.GetActiveView() as MapView;

            if (activeView != null)
                activeView.ZoomTo(_bmk);                
        }
    }
}

