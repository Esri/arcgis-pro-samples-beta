using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Internal.Core;
using ESRI.ArcGIS.ItemIndex;

namespace ProSDKSamples.ProjectItemProjectContainer
{

  /// <summary>
  /// Give the project an intuitive way to access project items in specific containers without having to know how to get a container
  /// </summary>
  public static class SampleProjectItemContainerExtensions
  {
    public static SampleProjectContainer GetSampleItems(this Project project)
    {
      return project.GetProjectItemContainer<SampleProjectContainer>("SampleProjectItems");
    }
  }


  // 
  //  The project container
  // 
  [ProjectViewModel(typeof(SampleProjectContainerViewModel))]
  public class SampleProjectContainer : ProjectItemContainer<SampleProjectItem>
  {
    // component Type passed to base constructor must match that defined in config.xml
    public SampleProjectContainer()
      : base("SampleProjectItems")
    {
      // NOOP
    }

    // create the child project item
    public override IProjectItem CreateItem(string name, string path, string type, string data)
    {
      var projectItem = new SampleProjectItem(new ItemInfoValue() { name = name, catalogPath = path });
      projectItem.PropertiesXml = data;
      this.Add(projectItem);
      return projectItem;
    }
  }

  /// <summary>
  ///  Viewmodel for the project container.  Key method is the GetMenu method which returns the context menu for the container. 
  /// </summary>
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
  //  The project item  (lives in the project container).  Always inherit from ArcGIS.Desktop.Catalog.ItemInfo
  //
  [ProjectViewModel(typeof(SampleProjectItemViewModel))]
  // ProjectCanRename attribute - project item can be renamed.  Rename can occur through F2 or the 'slow' rename. 
  //    ProjectCanRename attribute and implementing IProjectItemRename are linked together.
  //   Remove both the attribute and the IProjectItemRename interface if you do not want the item to allow renaming
  [ProjectCanRename("sampleItem")]
  // ProjectCanRemove attribute - project item can be removed.   Call the core remove method in the daml for a Remove button
  //    core Remove method is defined as "esri_core_module:RemoveProjectItem"  (see button with id esri_sampleItemRemoveButton in config.daml for an illustration)
  //   Remove this attribute if you do not want the item to be removed
  [ProjectCanRemove("sampleItem")]
  public class SampleProjectItem : ItemInfo, IProjectItemEdit, IProjectItemRename
  {
    public SampleProjectItem(ItemInfoValue iiv)
      : base(CleanItemInfo(iiv))
    {
      IsOpen = false;
      ValidateItem();
    }

    private static ItemInfoValue CleanItemInfo(ItemInfoValue iiv)
    {
      iiv.typeID = @"sampleItem";         // match the project item type id as was defined in config.xml
      iiv.type = "SampleProjectItems";    // match the parent container component type
      return iiv;
    }

    public bool IsOpen { get; set; }

    private void ValidateItem()
    {
      // TODO - set this.IsInvalid to true in order for projectItem to show invalid state
    }

    public override void OpenView()
    {
      // TODO : Add your custom implementation to open the project item - use this.Path as appropriate
    }

    #region IProjectItemEdit
    public void SetName(string newName)
    {
      // protected setter
      this.Name = newName;
    }

    public void SetPath(string path) { /* NOOP */ }
    public void SetDefault(bool isDefault)
    {
      this.IsDefault = isDefault;
    }

    public void SetInvalid(bool isInvalid)
    {
      this.IsInvalid = isInvalid;
    }

    public void SetSourceUri(string uri) { /* NOOP */ }
    public void SetSourceModifiedTime(TimeInstant time) { /* NOOP */ }

    #endregion

    #region RemoveFromProject
    public override Task RemoveFromProjectAsync()
    {
      // TODO - add any custom actions for your project item when it is removed from the project

      return base.RemoveFromProjectAsync();
    }
    #endregion

    #region IProjectItemRename

    public Task<ItemInfoValue> PreviewRenameAsync(string newName)
    {
      // TODO - add custom actions for rename if appropriate

      // change the item name
      var iiv = this.ItemInfoValue;
      iiv.name = newName;

      return Task.FromResult(iiv);
    }

    public Task RenameAsync(string newName)
    {
      // TODO - add any custom actions for renaming your project item

      // set the new name
      this.SetName(newName);

      // always succeed
      return Task.FromResult(0);
    }
    #endregion

  }

  /// <summary>
  /// Viewmodel for the project item
  /// </summary>
  internal sealed class SampleProjectItemViewModel : ItemInfoViewModel<SampleProjectItem>
  {
    public SampleProjectItemViewModel(SampleProjectItem projectItem)
      : base(projectItem)
    {
      // TODO : remove this if there is no double click action for the project item
      DoubleClickCommand = new RelayCommand(new Action<object>((p) => this.InvokeTask(p)), () => { return true; });
    }

    public override string ItemTypeID { get { return ItemInfoValue.typeID; } }
    public new string ItemTypeThumbnailID { get { return ItemInfoValue.typeID; } }

    #region double-click command

    private void InvokeTask(object arg)
    {
      ProjectItem.OpenView();
    }
    #endregion

    public override System.Windows.Controls.ContextMenu GetMenu
    {
      // TODO : return null if the project item will not have a context menu
      get { return FrameworkApplication.CreateContextMenu("SampleProjectItemContextMenu"); }
    }
  }
}
