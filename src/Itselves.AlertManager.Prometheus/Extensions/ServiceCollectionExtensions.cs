using System;
using System.Threading;
using System.Threading.Tasks;
using Itselves.AlertManager.Abstraction.Extensions;
using Itselves.AlertManager.Prometheus.Environment;
using Itselves.AlertManager.Prometheus.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Itselves.AlertManager.Prometheus.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPrometheusAlertManager(
        this IServiceCollection services,
        Action<PrometheusAlertManagerOptions>? configure = null)
    {
        services
            .TryAddSingleton<IDateTimeProvider, DefaultDateTimeProvider>();

        services
            .AddOptions<PrometheusAlertManagerOptions>()
            .BindConfiguration(nameof(PrometheusAlertManagerOptions))
            .Configure(opt => configure?.Invoke(opt))
            .Validate(
                opt => !string.IsNullOrEmpty(opt.MetricOptions.Name),
                "PrometheusAlertManagerOptions.MetricOptions.Name is required.")
            .Validate(
                opt => !opt.PreInitializeOptions.PreInitialize || opt.PreInitializeOptions.Alerts.Length > 0,
                "PrometheusAlertManagerOptions.PreInitializeOptions.Alerts must contain at least one alert.");

        services
            .TryAddSingleton<PrometheusAlertManager>();

        services
            .TryAddSingleton<ISupportWarmup>(sp => sp.GetRequiredService<PrometheusAlertManager>());

        services
            .AddAlertManager<PrometheusAlertManager>();

        services
            .AddHostedService<BackgroundWarmupWorker>();

        return services;
    }

    private sealed class DefaultDateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset GetNow() => DateTimeOffset.UtcNow;
    }

    private sealed class BackgroundWarmupWorker(ISupportWarmup supportWarmup) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken) => supportWarmup.Warmup(stoppingToken);
    }
}
