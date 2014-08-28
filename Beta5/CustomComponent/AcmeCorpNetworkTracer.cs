using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCSample.CustomComponentCategory;

namespace TheCustomComponent
{
    /// <summary>
    /// 3rd party trace extender. Registers itself in the <b></b>
    /// that is supported by the UCSamples.CoreImplementationTrace class.
    /// </summary>
    /// <remarks>When we are loaded, Framework will add us to the category.</remarks>
    public class AcmeCorpNetworkTracer : IComponentCategoryInterface {

        public Task Accept(ICoreImplementationTraceInterface currentTrace) {
            //check layer applies
            if (currentTrace.traceLayer.Name.CompareTo("MyCustomLayerName") == 0) {

                //store whatever state, etc.

                //start the ball rolling
                return currentTrace.Trace(this);
            }
            return Task.FromResult(0);
        }

        public IEnumerable<Stop> GetStops() {
            return new List<Stop>() {
                new Stop() { Name = "Stop 1"},
                new Stop() { Name = "Stop 2"},
                new Stop() { Name = "Stop 3"}
            };
        }

        public IEnumerable<Barrier> GetBarriers() {
            return new List<Barrier>() {
                new Barrier() { Name = "Barrier 1"},
                new Barrier() { Name = "Barrier 2"}
            };
        }

        public async Task ModifyTrace(INetworkResult results) {
            //custom tracing behavior
            await System.Threading.Tasks.Task.Delay(1000);
            //results
            TraceResult tr = new TraceResult();
            tr.tracedSegmentOids = new List<long>() { 10, 123, 15, 9, 345, 634 };
            results.UpdateResult(tr);
        }
    }
}
