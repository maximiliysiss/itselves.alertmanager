using System.Threading;
using System.Threading.Tasks;
using Itselves.AlertManager.Models;

namespace Itselves.AlertManager.Extensions;

public static class AlertManagerExtensions
{
    public static Task AlertAsync(this IAlertManager manager, string alert, CancellationToken cancellationToken)
        => manager.AlertAsync(new Alert(alert), cancellationToken);
}
