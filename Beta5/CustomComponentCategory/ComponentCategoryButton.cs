using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Internal.Mapping;

namespace UCSamples.CustomComponentCategory {
    class ComponentCategoryButton : Button {
        private ComponentCategoryUtils _helper = null;
        /// <summary>
        /// Load components if there are any and run the trace simulation
        /// </summary>
        protected override async void OnClick() {
            if (_helper == null) {
                _helper = new ComponentCategoryUtils();
            }
            _helper.RunTrace();
        }
    }
}