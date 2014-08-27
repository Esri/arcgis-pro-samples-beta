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
using System.Windows.Controls;
using System.Linq;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using System.Collections.ObjectModel;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System.ComponentModel;
using ArcGIS.Desktop.Core;
using System.Reflection;


namespace UCSamples.DockPaneDemo
{
    class DockpaneViewModel : DockPane, INotifyPropertyChanged
    {
        private ObservableCollection<BookmarkItem> _listBookmark = null;

         private bool _initialized = false;
         private static object _lockObject = new object();

        protected DockpaneViewModel()
        {
            _heading = "Bookmarks";
            _listBookmark = null;
            _isPaletteSupport = true;
        }

        #region Initialization

        ///<summary>
        /// Performs initialization when dock pane is opened.
        ///</summary>              
        protected async Task Initialize()
        {
            await base.InitializeAsync();
            ArcGISProjectOpenedEvents.Subscribe(OnProjectOpened);
            ArcGISProjectCreatedEvents.Subscribe(OnProjectOpened);
            ArcGISProjectClosedEvents.Subscribe(OnProjectClosed);
            OnProjectOpened(null);
            _initialized = true;
        }

        private async void OnProjectOpened(ArcGISProjectEventArgs eventArgs)
        {
            if (_listBookmark == null)
            {
                await LoadBookmarks();
            }
        }

        private void OnProjectClosed(ArcGISProjectEventArgs eventArgs)
        {
            lock (_lockObject)
            {
                _listBookmark.Clear();
                _listBookmark = null;
            }
            this.NotifyPropertyChanged(new PropertyChangedEventArgs("Cities"));
            this.NotifyPropertyChanged(new PropertyChangedEventArgs("HasBookmarksLoaded"));
        }
        /// <summary>
        /// Loads the bookmarks in the project
        /// </summary>
        /// <returns></returns>
        public async Task LoadBookmarks()
        {
            if (!_initialized)
            {
                //register for project events
                ArcGISProjectOpenedEvents.Unsubscribe(OnProjectOpened);
                ArcGISProjectCreatedEvents.Unsubscribe(OnProjectOpened);
                ArcGISProjectClosedEvents.Unsubscribe(OnProjectClosed);
                _initialized = true;
            }
            IList<Bookmark> bmks = await ProjectModule.CurrentProject.LoadBookmarksAsync();
            if (bmks.Count() == 0) {
                bmks = await LoadBookmarksFromMapxAsync();
            }
            lock (_lockObject)
            {
                _listBookmark = new ObservableCollection<BookmarkItem>();
                foreach (var bmk in bmks)
                    _listBookmark.Add(new BookmarkItem(bmk));
                
            }
            this.NotifyPropertyChanged(new PropertyChangedEventArgs("Cities"));
            this.NotifyPropertyChanged(new PropertyChangedEventArgs("HasBookmarksLoaded"));
        }

        /// <summary>
        /// Backup function to load bookmarks from a cached mapx file in case the
        /// loaded project does not have any
        /// </summary>
        private static Task<IList<Bookmark>> LoadBookmarksFromMapxAsync() {
            return QueuingTaskFactory.StartNew<IList<Bookmark>>( async () => {
                var assemblyPath = Assembly.GetExecutingAssembly().Location;
                var mapXPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assemblyPath), "SamplesData", "World.mapx");

                var project = ProjectModule.CurrentProject;
                var types = new string[1] { @"Map" };
                var paths = new string[1] { mapXPath };
                Tuple<string[], string[]> importResult = await project.ImportProjectItemAsync(types, paths, CancelableProgressor.None);
                var mapPath = importResult.Item2[0];

                var map = await MappingModule.GetMapAsync(mapPath);
                var bookmarks = await map.QueryBookmarks();
                List<Bookmark> bookMarks = new List<Bookmark>();
                foreach (Bookmark bmk in bookmarks) {
                    bookMarks.Add(bmk);
                }
                return bookMarks;
            });
        }

        #endregion

        #region properties

        private string _heading;
        /// <summary>
        /// Dock pane title
        /// </summary>
        public string Heading
        {
            get { return _heading; }
            set
            {
                SetProperty(ref _heading, value, () => Heading);
            }
        }
        /// <summary>
        /// Collection of bookmarks that will be displayed
        /// </summary>
        public IList<BookmarkItem> Cities
        {
            get { return _listBookmark; }
        }

        /// <summary>
        /// Gets the state of bookmarks loaded.
        /// </summary>
       
        public bool HasBookmarksLoaded
        {
            get
            {
                return _listBookmark == null ? false : _listBookmark.Count() > 0;
            }
        }

        private bool _isPaletteSupport;
        /// <summary>
        /// Gets and sets the Gallery view type for the bookmark dockpane
        /// </summary>
        public bool IsPaletteSupport
        {
            get { return _isPaletteSupport; }
            set
            {
                SetProperty(ref _isPaletteSupport, value, () => IsPaletteSupport);
            }
        }

        private bool _isBasicSupport;
        /// <summary>
        /// Gets and sets the List view type for the bookmark dockpane
        /// </summary>
        public bool IsBasicSupport
        {
            get { return _isBasicSupport; }
            set
            {
                SetProperty(ref _isBasicSupport, value, () => IsBasicSupport);
            }
        }

        private BookmarkItem _selectedItem;
        /// <summary>
        /// Gets and Sets the selected bookmark item.
        /// </summary>
        public BookmarkItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {

                _selectedItem = value;
                _selectedItem.ZoomTo();


                SetProperty(ref _selectedItem, value, () => SelectedItem);
                
            }
        }
        #endregion

        #region Burger Button
        /// <summary>
        /// Gets the tool tip for the burger button
        /// </summary>
        public string BurgerButtonTooltip
        {
            get { return "Options"; }
        }
        /// <summary>
        /// Gets the burger button context menu
        /// </summary>
        public ContextMenu BurgerButtonMenu
        {
            get { return FrameworkApplication.CreateContextMenu("DockpaneBurgerButton_Menu"); } //To do get place holder for ID
        }
        #endregion

    }
}
