using System.Threading;
using System.Threading.Tasks;
using Itselves.AlertManager.Abstraction;
using Itselves.AlertManager.Abstraction.Extensions;
using Itselves.AlertManager.ConsoleOnly.IntegrationTests.Shared.Fixture;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Itselves.AlertManager.ConsoleOnly.IntegrationTests.Cases;

[Collection(nameof(LocalCollectionFixture))]
public class ConsoleOnlyAlertManagerTests
{
    private readonly LocalFixture _fixture;

    public ConsoleOnlyAlertManagerTests(LocalFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AlertManager_ShouldSendAlert()
    {
        // Arrange
        var alertManager = _fixture.Services.GetRequiredService<IAlertManager>();

        // Act
        await alertManager.AlertAsync("name", CancellationToken.None);

        // Assert
    }
}
