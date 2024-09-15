using Microsoft.Extensions.Logging;

namespace Itselves.AlertManager.ConsoleOnly.Options;

public sealed class ConsoleOnlyAlertManagerOptions
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;
}
