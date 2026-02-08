using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Itselves.AlertManager.Abstraction;
using Itselves.AlertManager.Abstraction.Extensions;
using Itselves.AlertManager.Telegram.IntegrationTests.Shared.Extensions;
using Itselves.AlertManager.Telegram.IntegrationTests.Shared.Fixture;
using Itselves.AlertManager.Telegram.IntegrationTests.Shared.Wiremock;
using Itselves.AlertManager.Telegram.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestEase;
using Telegram.Bot;
using Telegram.Bot.Types;
using WireMock.Client;
using WireMock.Client.Extensions;
using Xunit;

namespace Itselves.AlertManager.Telegram.IntegrationTests.Cases;

[Collection(nameof(LocalCollectionFixture))]
public class TelegramAlertManagerTests(LocalFixture fixture)
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

        var alertOptions = fixture.Services.GetRequiredService<IOptions<TelegramAlertManagerOptions>>();

        var options = fixture.Services.GetRequiredService<IOptions<WiremockOptions>>();
        var wiremockClient = RestClient.For<IWireMockAdminApi>(options.Value.Url);

        var telegramResponse = new ApiResponse<Message>
        {
            Result = new Message(),
            Ok = true,
        };

        var body = JsonSerializer.Serialize(telegramResponse, JsonBotAPI.Options);

        var wiremockBuilder = wiremockClient.GetMappingBuilder();

        wiremockBuilder
            .Given(b => b
                .WithGuid(nameof(AlertAsync_ShouldAlertSuccess).AsGuid())
                .WithRequest(r => r.WithPath($"/bot{alertOptions.Value.BotToken}/sendMessage"))
                .WithResponse(r => r.WithStatusCode(200).WithBody(body))
                .Build());

        await wiremockBuilder.BuildAndPostAsync();

        // Act
        await alertManager.AlertAsync(message, CancellationToken.None);

        // Assert
    }
}
