using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ArcGIS.Desktop.Framework;
using UCSample.CustomComponentCategory;

namespace UCSamples.CustomComponentCategory {

    class CustomTraceComponent {
        public string Name;
        public string Description;
        public string Author;
        public string Version;
        public IComponentCategoryInterface extender;

    }

    /// <summary>
    /// Load your components, if there are any, that implement your category
    /// </summary>
    internal class ComponentCategoryUtils {

        private List<CustomTraceComponent> _customTracerExtenders = null;

        public void LoadCustomTraceExtenders() {
            if (_customTracerExtenders == null) {
                //load the components
                var extenderComponents = Categories.GetComponentElements("UCSampleExampleCategory");
                if (extenderComponents == null) {
                    System.Windows.MessageBox.Show("There are no custom trace extenders loaded for 'UCSampleExampleCategory'", "Category UCSampleExampleCategory");
                    return;
                }
                _customTracerExtenders = new List<CustomTraceComponent>();
                XNamespace ns = "http://schemas.esri.com/DADF/Registry";
                foreach (var component in extenderComponents) {
                    //get the content
                    XElement content = component.GetContent();
                    CustomTraceComponent record = new CustomTraceComponent();
                    record.Name = content.Attribute("name").Value;
                    record.Description = content.Attribute("description").Value;
                    record.Author = content.Attribute("author").Value;
                    record.Version = content.Attribute("version").Value;
                    //other parameters
                    XElement otherParam = content.Element(ns + "param1");
                    //get the value
                    string val = otherParam.Attribute("value").Value;
                    //etc.

                    //"the" component
                    record.extender = component.CreateComponent() as IComponentCategoryInterface;//this is your custom interface, 3rd parties MUST implement it
                    //save it
                    _customTracerExtenders.Add(record);
                }
            }
        }

        public async void RunTrace() {
            LoadCustomTraceExtenders();
            if (_customTracerExtenders == null)
                return;
            //run the trace
            StringBuilder results = new StringBuilder();
            using (CoreImplementationTrace tracer = new CoreImplementationTrace()) {

                results.AppendFormat("Start: {0}\r\n", DateTime.Now.ToString("G"));
                foreach (var record in _customTracerExtenders) {

                    results.AppendFormat("{0}\r\n", record.Author);
                    results.AppendFormat("{0},{1},{2}\r\n", record.Name, record.Description, record.Version);
                    await record.extender.Accept(tracer as ICoreImplementationTraceInterface);

                }
                results.AppendLine(tracer.Message());
                results.AppendFormat("End: {0}\r\n", DateTime.Now.ToString("G"));

            }
            System.Windows.MessageBox.Show(results.ToString(), "Trace Results");
        }

    }
}
