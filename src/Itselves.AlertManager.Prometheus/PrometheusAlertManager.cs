using System;
using System.Threading;
using System.Threading.Tasks;
using Itselves.AlertManager.Models;
using Itselves.AlertManager.Prometheus.Environment;
using Itselves.AlertManager.Prometheus.Exceptions;
using Itselves.AlertManager.Prometheus.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus.Client;

namespace Itselves.AlertManager.Prometheus;

internal sealed class PrometheusAlertManager : IAlertManager, ISupportWarmup
{
    private readonly IMetricFamily<ICounter> _alertStream;

    private readonly ILogger<PrometheusAlertManager> _logger;

    private readonly IDateTimeProvider _dateTimeProvider;

    private readonly PrometheusAlertManagerOptions _options;

    private readonly DateTimeOffset _initalDateTime;

    public PrometheusAlertManager(
        IMetricFactory metricFactory,
        IOptions<PrometheusAlertManagerOptions> options,
        ILogger<PrometheusAlertManager> logger,
        IDateTimeProvider dateTimeProvider)
    {
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;

        _options = options.Value;

        _initalDateTime = _dateTimeProvider.GetNow();

        var metricOptions = _options.MetricOptions;

        _alertStream = metricFactory.CreateCounter(
            name: metricOptions.Name,
            help: metricOptions.Description,
            labelNames: [metricOptions.Label]);
    }

    public Task AlertAsync(Alert alert, CancellationToken cancellationToken)
    {
        var diff = _dateTimeProvider.GetNow() - _initalDateTime;
        if (diff < _options.InitialDelay)
        {
            // Call delayed send alerting by cycle collect
            var delay = _options.InitialDelay - diff;
            _ = Task.Run(() => AlertAsync(alert, delay, cancellationToken), cancellationToken);

            return Task.CompletedTask;
        }

        _logger.Log(_options.LogLevel, "Alert: {Alert}", alert);

        _alertStream
            .WithLabels(alert.Name)
            .Inc();

        return Task.CompletedTask;
    }

    private async Task AlertAsync(Alert alert, TimeSpan delay, CancellationToken cancellationToken)
    {
        await Task.Delay(delay, cancellationToken);
        await AlertAsync(alert, cancellationToken);
    }

    public Task Warmup(CancellationToken cancellationToken)
    {
        var preInitializeOptions = _options.PreInitializeOptions;

        if (preInitializeOptions.PreInitialize is false)
            return Task.CompletedTask;

        if (preInitializeOptions.Alerts.Length is 0)
            throw new NotFoundAlertsException();

        foreach (var alert in preInitializeOptions.Alerts)
            _alertStream.WithLabels(alert).Reset();

        return Task.CompletedTask;
    }
}
