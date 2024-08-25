using System.Threading;
using System.Threading.Tasks;

namespace Itselves.AlertManager.Prometheus.Environment;

internal interface ISupportWarmup
{
    Task Warmup(CancellationToken cancellationToken);
}
