using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Itselves.AlertManager.Abstraction.Extensions;
using Itselves.AlertManager.Abstraction.IntegrationTests.Shared.Fixture;
using Itselves.AlertManager.Abstraction.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Itselves.AlertManager.Abstraction.IntegrationTests.Cases;

[Collection(nameof(LocalCollectionFixture))]
public class SeveralManagersTests(LocalFixture fixture)
{
    [Fact]
    public async Task AlertAsync_ShouldCallBothManagers()
    {
        // Arrange
        var manager = fixture.Services.GetRequiredService<IAlertManager>();

        // Act
        await manager.AlertAsync("alert", CancellationToken.None);

        // Assert
        FirstAlertManager.Counter.Should().Be(1);
        SecondAlertManager.Counter.Should().Be(1);
    }

    public sealed class FirstAlertManager : IAlertManager
    {
        private static int _counter;
        public static int Counter => _counter;

        public Task AlertAsync(Alert alert, CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _counter);
            return Task.CompletedTask;
        }
    }

    public sealed class SecondAlertManager : IAlertManager
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
