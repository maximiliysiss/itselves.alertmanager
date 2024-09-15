using System;
using System.Threading;
using System.Threading.Tasks;
using Itselves.AlertManager.Abstraction.Extensions;
using Itselves.AlertManager.ConsoleOnly.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Itselves.AlertManager.ConsoleOnly.Tests;

using Format = Func<It.IsAnyType, Exception?, string>;

public class ConsoleOnlyAlertManagerTests
{
    [Fact]
    public async Task AlertAsync_ShouldSendAlert()
    {
        // Arrange
        const LogLevel logLevel = LogLevel.Critical;

        var options = new ConsoleOnlyAlertManagerOptions { LogLevel = logLevel };

        var logger = new Mock<ILogger<ConsoleOnlyAlertManager>>(MockBehavior.Strict);
        logger
            .Setup(c => c.Log(logLevel, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception?>(), It.IsAny<Format>()));

        var manager = Create(logger, options);

        // Act
        await manager.AlertAsync("name", CancellationToken.None);

        // Assert
        logger.VerifyAll();
    }

    private static ConsoleOnlyAlertManager Create(
        Mock<ILogger<ConsoleOnlyAlertManager>>? logger = null,
        ConsoleOnlyAlertManagerOptions? options = null)
    {
        return new ConsoleOnlyAlertManager(
            logger?.Object ?? NullLogger<ConsoleOnlyAlertManager>.Instance,
            Microsoft.Extensions.Options.Options.Create(options ?? new ConsoleOnlyAlertManagerOptions()));
    }
}
