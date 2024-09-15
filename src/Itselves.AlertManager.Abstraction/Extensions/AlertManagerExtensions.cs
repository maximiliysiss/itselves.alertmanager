using System.Threading;
using System.Threading.Tasks;
using Itselves.AlertManager.Abstraction.Models;

namespace Itselves.AlertManager.Abstraction.Extensions;

public static class AlertManagerExtensions
{
    public static Task AlertAsync(this IAlertManager manager, string alert, CancellationToken cancellationToken)
        => manager.AlertAsync(new StringAlert(alert), cancellationToken);

    private sealed class StringAlert(string name) : Alert(name)
    {
        public override string ToString() => Name;
    }
}
