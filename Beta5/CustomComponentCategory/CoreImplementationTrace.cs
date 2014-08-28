using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace UCSample.CustomComponentCategory {
    /// <summary>
    /// Simulates your &quot;core&quot; implementation that can be extended via your
    /// <b>UCSampleExampleCategory</b> category. Vendors that extend your category MUST implement the 
    /// <see cref="IComponentCategoryInterface"/> interface.
    /// </summary>
    /// <remarks>We simulate some kind of tracing algorithm that can be extended by
    /// 3rd parties</remarks>
    internal class CoreImplementationTrace : ICoreImplementationTraceInterface, INetworkResult, IDisposable {

        private StringBuilder _sb = null;
        private TraceResult _tr = null;
        /// <summary>
        /// For Beta4, NetworkLayer type is not supported so we have
        /// a stand-in here
        /// </summary>
        public NetworkLayer traceLayer {
            get { return new NetworkLayer() { Name = "MyCustomLayerName" }; }
        }
        /// <summary>
        /// Execute the trace with a custom component that can extend it
        /// </summary>
        /// <param name="extender"></param>
        /// <returns></returns>
        public Task Trace(IComponentCategoryInterface extender) {
            _sb = null;
            _sb = new StringBuilder();
            return QueuingTaskFactory.StartNew(async () => {
                
                //get the stops
                foreach (var stop in extender.GetStops()) {
                    _sb.AppendFormat("Stop: {0}\r\n", stop.Name);
                }
                //barriers
                foreach (var barrier in extender.GetBarriers()) {
                    _sb.AppendFormat("Barrier: {0}\r\n", barrier.Name);
                }
                //do whatever
                await System.Threading.Tasks.Task.Delay(1000);
                //extenders turn
                await extender.ModifyTrace(this);
                //results
                string sep = "";
                _sb.AppendLine("\r\nResults");
                _sb.Append("OIDS: ");
                foreach (var oid in _tr.tracedSegmentOids) {
                    _sb.Append(sep + oid.ToString());
                    sep = ",";
                }
            });

        }

        public void UpdateResult(TraceResult results) {
            if (_tr == null)
                _tr = new TraceResult();
            _tr.tracedSegmentOids = new List<long>();
            foreach (var oid in results.tracedSegmentOids)
                _tr.tracedSegmentOids.Add(oid);
        }

        public string Message() {
            return _sb == null ? "No results" : _sb.ToString();
        }

        public void Dispose() {
            _tr = null;
            _sb = null;
        }
    }
}
