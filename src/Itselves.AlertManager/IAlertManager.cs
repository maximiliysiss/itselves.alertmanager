using System.Threading;
using System.Threading.Tasks;
using Itselves.AlertManager.Models;

namespace Itselves.AlertManager;

public interface IAlertManager
{
    Task AlertAsync(Alert alert, CancellationToken cancellationToken);
}
