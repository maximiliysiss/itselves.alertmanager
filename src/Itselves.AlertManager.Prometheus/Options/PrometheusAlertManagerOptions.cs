using System;
using Microsoft.Extensions.Logging;

namespace Itselves.AlertManager.Prometheus.Options;

public sealed class PrometheusAlertManagerOptions
{
    /// <summary>
    /// Options for Prometheus metric
    /// </summary>
    public PrometheusMetricOptions MetricOptions { get; set; } = new();

    /// <summary>
    /// Pre initialize options
    /// </summary>
    public PrometheusPreInitializeOptions PreInitializeOptions { get; set; } = new();

    /// <summary>
    /// Delay for collect cycle of prometheus
    /// </summary>
    public TimeSpan InitialDelay { get; set; } = TimeSpan.FromSeconds(30);

    public LogLevel LogLevel { get; set; } = LogLevel.Warning;
}
