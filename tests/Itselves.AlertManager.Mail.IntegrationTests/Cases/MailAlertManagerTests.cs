using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Itselves.AlertManager.Abstraction;
using Itselves.AlertManager.Abstraction.Extensions;
using Itselves.AlertManager.Mail.IntegrationTests.Shared.Fixture;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Itselves.AlertManager.Mail.IntegrationTests.Cases;

[Collection(nameof(LocalCollectionFixture))]
public class MailAlertManagerTests(LocalFixture fixture)
{
    [Fact]
    public void GetRequiredService_ShouldResolveAlertManager()
    {
        // Arrange
        var serviceProvider = fixture.Services;

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

        var alertManager = fixture.Services.GetRequiredService<IAlertManager>();

        // Act
        await alertManager.AlertAsync(message, CancellationToken.None);

        // Assert
    }
}
