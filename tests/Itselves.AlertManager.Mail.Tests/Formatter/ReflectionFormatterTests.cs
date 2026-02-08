using System;
using System.Collections.Generic;
using FluentAssertions;
using Itselves.AlertManager.Abstraction.Models;
using Itselves.AlertManager.Mail.Environment;
using Itselves.AlertManager.Mail.Formatter;
using Xunit;

namespace Itselves.AlertManager.Mail.Tests.Formatter;

public class ReflectionFormatterTests
{
    [Theory]
    [MemberData(nameof(Format_ShouldWorks_SampleData))]
    public void Format_ShouldWorks(string input, Alert alert, string expected)
    {
        // Arrange
        var formatter = new ReflectionFormatter(new DateTimeProvider());

        // Act
        var format = formatter.Format(input, alert);

        // Assert
        format.Should().Be(expected);
    }

    public static IEnumerable<object[]> Format_ShouldWorks_SampleData()
    {
        yield return ["Hello, {Alert}!", new StringAlert("Test"), "Hello, Test!"];
        yield return ["Hello, {Alert}, {Alert}!", new StringAlert("Test"), "Hello, Test, Test!"];
        yield return ["Hello, {Alert}, {UtcNow}!", new StringAlert("Test"), $"Hello, Test, {DateTimeProvider.Now}!"];
        yield return ["Hello, {Alert} {CustomProperty}!", new CustomAlert("Test", "Custom"), "Hello, Test Custom!"];
    }

    private sealed class StringAlert(string name) : Alert(name)
    {
        public override string ToString() => Name;
    }

    private sealed class CustomAlert(string name, string customProperty) : Alert(name)
    {
        public string CustomProperty { get; set; } = customProperty;
        public override string ToString() => Name;
    }

    private sealed class DateTimeProvider : IDateTimeProvider
    {
        public static readonly DateTimeOffset Now = DateTimeOffset.UtcNow;
        public DateTimeOffset GetNow() => Now;
    }
}
