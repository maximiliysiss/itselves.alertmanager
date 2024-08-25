namespace Itselves.AlertManager.Prometheus.Options;

public sealed class PrometheusMetricOptions
{
    /// <summary>
    /// Name of alerts in prometheus
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Hint/Description of prometheus alert
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Label of name for metric
    /// </summary>
    public string Label { get; set; } = "alert";
}
