using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCSample.CustomComponentCategory {
    public class Stop {
        public string Name { get; set; }
        public int capacity { get; set; }
    }
    public class Barrier {
        public string Name { get; set; }
    }
    public class TraceResult {
        public List<long> tracedSegmentOids { get; set; }
    }
    public class NetworkLayer {
        public string Name { get; set; }
    }
    /// <summary>
    /// Some CORE interface for dealing with results
    /// </summary>
    public interface INetworkResult {
        void UpdateResult(TraceResult results);
    }
    /// <summary>
    /// Your CORE interface - e.g. Here we simulate something for a fictitious tracing algorithm.
    /// You do not need to use an interface.....how you consume your category components is up to
    /// you
    /// </summary>
    /// <remarks>For the purposes of this sample, ICoreImplementationTraceInterface has 
    /// an extensibility point "Trace()" that you have implemented to allow 3rd
    /// parties to extend your imaginary tracing capabilities. 3rd parties would have to
    /// implement the <b>ISomeInterfaceThat3rdPartiesImplement</b> and would have registered
    /// their Addins within your <b>UCSampleExampleCategory</b> category.<para></para>
    /// When Framework loads all the addins it will keep tabs on any categories (yours or others) that
    /// Addins implement. You can ask Framework for the components it has loaded that implement your
    /// category at Runtime.<para></para>We are using a simple Visitor
    /// pattern to illustrate. Basically, the <b>extender</b> parameter would be a 
    /// component loaded from a 3rd party Add-in dll that had registered in your category.</remarks>
    public interface ICoreImplementationTraceInterface {
        NetworkLayer traceLayer { get; }
        Task Trace(IComponentCategoryInterface extender);
    }
}
