using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Itselves.AlertManager.Abstraction;
using Itselves.AlertManager.Abstraction.Extensions;
using Itselves.AlertManager.Prometheus.IntegrationTests.Shared.Fixture;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Itselves.AlertManager.Prometheus.IntegrationTests.Cases;

[Collection(nameof(LocalCollectionFixture))]
public sealed class PrometheusAlertManagerTests
{
    private readonly LocalFixture _fixture;

    public PrometheusAlertManagerTests(LocalFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void GetRequiredService_ShouldResolveAlertManager()
    {
        // Arrange
        var serviceProvider = _fixture.Services;

        // Act
        var alertManager = serviceProvider.GetRequiredService<IAlertManager>();

        // Assert
        alertManager.Should().NotBeNull();
    }

    [Fact]
    public async Task AlertAsync_ShouldAlertSuccess()
    {
        // Arrange
        const string message = "message";

        var alertManager = _fixture.Services.GetRequiredService<IAlertManager>();

        // Act
        await alertManager.AlertAsync(message, CancellationToken.None);

        // Assert
    }
}
