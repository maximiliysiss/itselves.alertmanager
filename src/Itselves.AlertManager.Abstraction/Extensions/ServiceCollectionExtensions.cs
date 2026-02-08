using System;
using System.Threading;
using System.Threading.Tasks;
using Itselves.AlertManager.Abstraction.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Itselves.AlertManager.Abstraction.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAlertManager<T>(this IServiceCollection services) where T : class, IAlertManager
    {
        services
            .TryAddSingleton<IAlertManager, Caller.AlertManager>();

        services
            .TryAddEnumerable(ServiceDescriptor.Singleton<IAlertHandler, AlertHandler<T>>());

        return services;
    }

    private sealed class AlertHandler<T>(IServiceProvider serviceProvider) : IAlertHandler where T : class, IAlertManager
    {
        private readonly T _manager = serviceProvider.GetService<T>() ?? ActivatorUtilities.CreateInstance<T>(serviceProvider);
        public Task AlertAsync(Alert alert, CancellationToken cancellationToken) => _manager.AlertAsync(alert, cancellationToken);
    }
}
