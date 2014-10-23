using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCSample.CustomComponentCategory {

    /// <summary>
    /// Implement this interface to extend the core network tracing capabilities.
    /// The name of the interface is <b>arbitrary</b>. Call it what you want.
    /// </summary>
    /// <remarks>Providers must register themselves in the NetworkTraceExtender component
    /// category</remarks>
    public interface IComponentCategoryInterface {
        Task Accept(ICoreImplementationTraceInterface currentTrace);
        IEnumerable<Stop> GetStops();
        IEnumerable<Barrier> GetBarriers();
        Task ModifyTrace(INetworkResult results);
    }
}
