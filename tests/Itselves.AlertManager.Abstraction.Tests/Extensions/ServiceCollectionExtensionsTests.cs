using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Itselves.AlertManager.Abstraction.Extensions;
using Itselves.AlertManager.Abstraction.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Itselves.AlertManager.Abstraction.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddAlertManager_ShouldAddSeveralAlertManagers()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services
            .AddAlertManager<FirstAlertManager>()
            .AddAlertManager<SecondAlertManager>();

        // Assert
        services.Where(c => c.ServiceType == typeof(IAlertManager)).Should().ContainSingle();
        services.Where(c => c.ServiceType == typeof(IAlertHandler)).Should().HaveCount(2);

        services.Where(c => ContainsType(c, typeof(FirstAlertManager))).Should().ContainSingle();
        services.Where(c => ContainsType(c, typeof(SecondAlertManager))).Should().ContainSingle();
    }

    [Fact]
    public void AddAlertManager_ShouldAddOnlyOneTime_WhenTryToDuplicate()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services
            .AddAlertManager<FirstAlertManager>()
            .AddAlertManager<FirstAlertManager>();

        // Assert
        services.Where(c => c.ServiceType == typeof(IAlertManager)).Should().ContainSingle();
        services.Where(c => c.ServiceType == typeof(IAlertHandler)).Should().ContainSingle();

        services.Where(c => ContainsType(c, typeof(FirstAlertManager))).Should().ContainSingle();
    }

    [Fact]
    public async Task AlertAsync_ShouldCallAllRegisteredManagers()
    {
        // Arrange
        var services = new ServiceCollection();

        services
            .AddAlertManager<FirstAlertManager>()
            .AddAlertManager<SecondAlertManager>();

        var serviceProvider = services.BuildServiceProvider();

        // Act
        var alertManager = serviceProvider.GetRequiredService<IAlertManager>();

        await alertManager.AlertAsync("Alert", CancellationToken.None);

        // Assert
        FirstAlertManager.Counter.Should().Be(1);
        SecondAlertManager.Counter.Should().Be(1);
    }

    private static bool ContainsType(ServiceDescriptor descriptor, Type type)
    {
        return descriptor.ImplementationType?.IsGenericType is true &&
               descriptor.ImplementationType?.GetGenericArguments().Contains(type) is true;
    }

    private sealed class FirstAlertManager : IAlertManager
    {
        private static int _counter;

        public static int Counter => _counter;

        public Task AlertAsync(Alert alert, CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _counter);
            return Task.CompletedTask;
        }
    }

    private sealed class SecondAlertManager : IAlertManager
    {
        private static int _counter;

        public static int Counter => _counter;

        public Task AlertAsync(Alert alert, CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _counter);
            return Task.CompletedTask;
        }
    }
}
