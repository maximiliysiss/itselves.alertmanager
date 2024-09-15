using System.Threading;
using System.Threading.Tasks;
using Itselves.AlertManager.Abstraction.Models;

namespace Itselves.AlertManager.Abstraction;

public interface IAlertManager
{
    Task AlertAsync(Alert alert, CancellationToken cancellationToken);
}
