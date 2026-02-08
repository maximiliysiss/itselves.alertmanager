using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Itselves.AlertManager.Abstraction.Models;

namespace Itselves.AlertManager.Abstraction.Caller;

internal sealed class AlertManager(IEnumerable<IAlertHandler> handlers) : IAlertManager
{
    public Task AlertAsync(Alert alert, CancellationToken cancellationToken)
        => Task.WhenAll(handlers.Select(x => x.AlertAsync(alert, cancellationToken)));
}
