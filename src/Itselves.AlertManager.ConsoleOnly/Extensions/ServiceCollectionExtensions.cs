using System;
using Itselves.AlertManager.Abstraction;
using Itselves.AlertManager.ConsoleOnly.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Itselves.AlertManager.ConsoleOnly.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleOnlyAlertManager(
        this IServiceCollection services,
        Action<ConsoleOnlyAlertManagerOptions>? configure = null)
    {
        services
            .AddOptions<ConsoleOnlyAlertManagerOptions>()
            .BindConfiguration(nameof(ConsoleOnlyAlertManagerOptions))
            .Configure(opt => configure?.Invoke(opt));

        services
            .AddSingleton<IAlertManager, ConsoleOnlyAlertManager>();

        return services;
    }
}
