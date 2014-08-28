using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ESRI.ArcGIS.ItemIndex;
using System;
using ArcGIS.Desktop.Internal.Core;

namespace ProSDKSamples.ProjectItemProjectContainer
{
    // 
    //  The project container
    // 
    [ProjectViewModel(typeof(SampleProjectContainerViewModel))]
    internal class SampleProjectContainer : ProjectItemContainer<SampleProjectItem>
    {
        // component Type passed to base object must match that defined in config.xml
        public SampleProjectContainer() : base("SampleProjectItems")
        {
            // NOOP
        }

        // create the child project item
        public override SampleProjectItem CreateItem(string name, string path, string type, string data)
        {
            var projectItem = new SampleProjectItem(new ItemInfoValue() { name = name, catalogPath = path });
            this.Add(projectItem);
            return projectItem;
        }
    }

    //
    //  viewModel for the project container
    //
    internal sealed class SampleProjectContainerViewModel : ProjectItemContainerViewModel<SampleProjectContainer>
    {
        public SampleProjectContainerViewModel(SampleProjectContainer container)
            : base(container)
        {
            // no-op
        }

        public override System.Windows.Controls.ContextMenu GetMenu
        {
            // TODO : return null if the project container will not have a context menu
            get { return FrameworkApplication.CreateContextMenu("SampleProjectContainerContextMenu"); }
        }
    }


    //
    //  The project item  (lives in the project container)
    //
    [ProjectViewModel(typeof(SampleProjectItemViewModel))]
    internal class SampleProjectItem : ItemInfo
    {
        public SampleProjectItem(ItemInfoValue iiv) : base(CleanItemInfo(iiv))
        {
            IsOpen = false;
        }

        private static ItemInfoValue CleanItemInfo(ItemInfoValue iiv)
        {
            iiv.typeID = @"sampleItem";         // match the project item type id as was defined in config.xml
            iiv.type = "SampleProjectItems";    // match the parent container component type
            return iiv;
        }

        public bool IsOpen { get; set; }

        public override void RemoveFromProject()
        {
          (ProjectModule.CurrentProject as IInternalGISProjectItem).RemoveProjectItemAsync("SampleProjectItems", this.Path);
        }

        public override void OpenView()
        {
            // TODO : Add your custom implementation to open the project item - use this.Path as appropriate
        }
    }

    //
    // viewModel for the project item
    //
    internal sealed class SampleProjectItemViewModel : ItemInfoViewModel<SampleProjectItem>
    {
        public SampleProjectItemViewModel(SampleProjectItem projectItem) : base(projectItem)
        {
            // TODO : remove this if there is no double click action for the project item
            DoubleClickCommand = new RelayCommand(new Action<object>((p) => this.InvokeTask(p)), () => { return true; });
        }

        public override string ItemTypeID { get { return ItemInfoValue.typeID; } }
        public new string ItemTypeThumbnailID { get { return ItemInfoValue.typeID; } }

        #region double-click command

        private void InvokeTask(object arg)
        {
            OpenItem();
        }
        #endregion

        public override System.Windows.Controls.ContextMenu GetMenu
        {
            // TODO : return null if the project item will not have a context menu
            get { return FrameworkApplication.CreateContextMenu("SampleProjectItemContextMenu"); }
        }

        internal void OpenItem()
        {
            _projectItem.OpenView();
        }

        public override void RemoveFromProject()
        {
            _projectItem.RemoveFromProject();
        }
    }
}
