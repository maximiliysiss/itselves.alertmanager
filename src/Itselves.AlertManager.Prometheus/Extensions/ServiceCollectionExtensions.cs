using System;
using System.Threading;
using System.Threading.Tasks;
using Itselves.AlertManager.Prometheus.Environment;
using Itselves.AlertManager.Prometheus.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Itselves.AlertManager.Prometheus.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPrometheusAlertManager(
        this IServiceCollection services,
        Action<PrometheusAlertManagerOptions>? configure = null)
    {
        services
            .AddSingleton<IDateTimeProvider, DefaultDateTimeProvider>();

        services
            .AddOptions<PrometheusAlertManagerOptions>()
            .BindConfiguration(nameof(PrometheusAlertManagerOptions))
            .Configure(opt => configure?.Invoke(opt));

        services
            .AddSingleton<IAlertManager, PrometheusAlertManager>();

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
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await supportWarmup.Warmup(stoppingToken);
        }
    }
}
