using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Itselves.AlertManager.Extensions;
using Itselves.AlertManager.Prometheus.Environment;
using Itselves.AlertManager.Prometheus.Exceptions;
using Itselves.AlertManager.Prometheus.Options;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Prometheus.Client;
using Xunit;

namespace Itselves.AlertManager.Prometheus.Tests;

public sealed class PrometheusAlertManagerTests
{
    [Fact]
    public async Task Warmup_ShouldDoNothing_WhenPreInitIsOff()
    {
        // Arrange
        var options = new PrometheusAlertManagerOptions
        {
            PreInitializeOptions = new PrometheusPreInitializeOptions
            {
                PreInitialize = false
            }
        };

        var manager = Create(options: options);

        // Act
        await manager.Warmup(CancellationToken.None);

        // Assert
    }

    [Fact]
    public async Task Warmup_ShouldThrowException_WhenNotFoundAlertsForInit()
    {
        // Arrange
        var options = new PrometheusAlertManagerOptions
        {
            PreInitializeOptions = new PrometheusPreInitializeOptions
            {
                PreInitialize = true,
                Alerts = []
            }
        };

        var manager = Create(options: options);

        // Act
        var action = () => manager.Warmup(CancellationToken.None);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundAlertsException>()
            .WithMessage("Not found any registered alerts");
    }

    [Fact]
    public async Task Warmup_ShouldWorkSuccess_WhenThereAreAllInfoForInit()
    {
        // Arrange
        const string alertName = nameof(Warmup_ShouldWorkSuccess_WhenThereAreAllInfoForInit);

        var options = new PrometheusAlertManagerOptions
        {
            PreInitializeOptions = new PrometheusPreInitializeOptions
            {
                PreInitialize = true,
                Alerts = [alertName]
            }
        };

        var counter = new Mock<ICounter>(MockBehavior.Strict);
        counter
            .Setup(c => c.Reset());

        var family = new Mock<IMetricFamily<ICounter>>(MockBehavior.Strict);
        family
            .Setup(c => c.WithLabels(alertName))
            .Returns(counter.Object);

        var metricOptions = options.MetricOptions;

        var metricFactory = new Mock<IMetricFactory>(MockBehavior.Strict);
        metricFactory
            .Setup(c => c.CreateCounter(metricOptions.Name, metricOptions.Description, new[] { metricOptions.Label }))
            .Returns(family.Object);

        var manager = Create(metricFactory, options: options);

        // Act
        await manager.Warmup(CancellationToken.None);

        // Assert
        Mock.VerifyAll(family, metricFactory, counter);
    }

    [Fact]
    public async Task AlertAsync_ShouldCallInc_WhenCallAfterDelay()
    {
        // Arrange
        const string alertName = nameof(AlertAsync_ShouldNotCallInc_WhenDelayIsNotEnd);

        var options = new PrometheusAlertManagerOptions();

        var counter = new Mock<ICounter>(MockBehavior.Strict);
        counter
            .Setup(c => c.Inc());

        var family = new Mock<IMetricFamily<ICounter>>(MockBehavior.Strict);
        family
            .Setup(c => c.WithLabels(alertName))
            .Returns(counter.Object);

        var metricOptions = options.MetricOptions;

        var metricFactory = new Mock<IMetricFactory>(MockBehavior.Strict);
        metricFactory
            .Setup(c => c.CreateCounter(metricOptions.Name, metricOptions.Description, new[] { metricOptions.Label }))
            .Returns(family.Object);

        var initalDateTime = DateTimeOffset.UtcNow;
        var callDateTime = initalDateTime.Add(options.InitialDelay).AddSeconds(1);

        var dateTimeProvider = new Mock<IDateTimeProvider>(MockBehavior.Strict);
        dateTimeProvider
            .SetupSequence(c => c.GetNow())
            .Returns(initalDateTime)
            .Returns(callDateTime);

        var manager = Create(metricFactory, dateTimeProvider, options: options);

        // Act
        await manager.AlertAsync(alertName, CancellationToken.None);

        // Assert
        Mock.VerifyAll(family, metricFactory, counter, dateTimeProvider);
    }

    [Fact]
    public async Task AlertAsync_ShouldCallInc_AfterDelay()
    {
        // Arrange
        const string alertName = nameof(AlertAsync_ShouldNotCallInc_WhenDelayIsNotEnd);

        var options = new PrometheusAlertManagerOptions { InitialDelay = TimeSpan.FromSeconds(5) };

        var counter = new Mock<ICounter>(MockBehavior.Strict);
        counter
            .Setup(c => c.Inc());

        var family = new Mock<IMetricFamily<ICounter>>(MockBehavior.Strict);
        family
            .Setup(c => c.WithLabels(alertName))
            .Returns(counter.Object);

        var metricOptions = options.MetricOptions;

        var metricFactory = new Mock<IMetricFactory>(MockBehavior.Strict);
        metricFactory
            .Setup(c => c.CreateCounter(metricOptions.Name, metricOptions.Description, new[] { metricOptions.Label }))
            .Returns(family.Object);

        var initalDateTime = DateTimeOffset.UtcNow;
        var callDateTime = initalDateTime.AddSeconds(1);
        var afterDelatDateTime = callDateTime.Add(options.InitialDelay);

        var dateTimeProvider = new Mock<IDateTimeProvider>(MockBehavior.Strict);
        dateTimeProvider
            .SetupSequence(c => c.GetNow())
            .Returns(initalDateTime)
            .Returns(callDateTime)
            .Returns(afterDelatDateTime);

        var manager = Create(metricFactory, dateTimeProvider, options: options);

        var callDelay = options.InitialDelay.Add(TimeSpan.FromSeconds(2));

        // Act
        await manager.AlertAsync(alertName, CancellationToken.None);
        await Task.Delay(callDelay);

        // Assert
        Mock.VerifyAll(family, metricFactory, counter, dateTimeProvider);
    }

    [Fact]
    public async Task AlertAsync_ShouldNotCallInc_WhenDelayIsNotEnd()
    {
        // Arrange
        const string alertName = nameof(AlertAsync_ShouldNotCallInc_WhenDelayIsNotEnd);

        var options = new PrometheusAlertManagerOptions { InitialDelay = TimeSpan.FromSeconds(5) };

        var family = new Mock<IMetricFamily<ICounter>>(MockBehavior.Strict);

        var metricOptions = options.MetricOptions;

        var metricFactory = new Mock<IMetricFactory>(MockBehavior.Strict);
        metricFactory
            .Setup(c => c.CreateCounter(metricOptions.Name, metricOptions.Description, new[] { metricOptions.Label }))
            .Returns(family.Object);

        var initalDateTime = DateTimeOffset.UtcNow;
        var callDateTime = initalDateTime.AddSeconds(1);
        var afterDelatDateTime = callDateTime.Add(options.InitialDelay);

        var dateTimeProvider = new Mock<IDateTimeProvider>(MockBehavior.Strict);
        dateTimeProvider
            .SetupSequence(c => c.GetNow())
            .Returns(initalDateTime)
            .Returns(callDateTime)
            .Returns(afterDelatDateTime);

        var manager = Create(metricFactory, dateTimeProvider, options: options);

        // Act
        await manager.AlertAsync(alertName, CancellationToken.None);

        // Assert
        family
            .Verify(c => c.WithLabels(It.IsAny<string[]>()), Times.Never);

        Mock.VerifyAll(family, metricFactory, dateTimeProvider);
    }

    private static PrometheusAlertManager Create(
        Mock<IMetricFactory>? metricFactory = null,
        Mock<IDateTimeProvider>? dateTimeProvider = null,
        PrometheusAlertManagerOptions? options = null) => new(
        metricFactory: metricFactory?.Object ?? Mock.Of<IMetricFactory>(),
        options: Microsoft.Extensions.Options.Options.Create(options ?? new PrometheusAlertManagerOptions()),
        logger: NullLogger<PrometheusAlertManager>.Instance,
        dateTimeProvider: dateTimeProvider?.Object ?? Mock.Of<IDateTimeProvider>());
}
