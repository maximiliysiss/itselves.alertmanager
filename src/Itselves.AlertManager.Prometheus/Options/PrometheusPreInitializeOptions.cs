namespace Itselves.AlertManager.Prometheus.Options;

public sealed class PrometheusPreInitializeOptions
{
    /// <summary>
    /// Should preinitialize by 0 alerts
    /// </summary>
    public bool PreInitialize { get; set; } = false;

    /// <summary>
    /// Preinitialized alerts names
    /// </summary>
    public string[] Alerts { get; set; } = [];
}
