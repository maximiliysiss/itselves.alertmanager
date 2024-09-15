using System.Threading;
using System.Threading.Tasks;
using Itselves.AlertManager.Abstraction;
using Itselves.AlertManager.Abstraction.Models;
using Itselves.AlertManager.ConsoleOnly.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Itselves.AlertManager.ConsoleOnly;

internal sealed class ConsoleOnlyAlertManager : IAlertManager
{
    private readonly ILogger<ConsoleOnlyAlertManager> _logger;

    private readonly ConsoleOnlyAlertManagerOptions _options;

    public ConsoleOnlyAlertManager(ILogger<ConsoleOnlyAlertManager> logger, IOptions<ConsoleOnlyAlertManagerOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public Task AlertAsync(Alert alert, CancellationToken cancellationToken)
    {
        _logger.Log(_options.LogLevel, $"Alert: {alert}", alert);
        return Task.CompletedTask;
    }
}
