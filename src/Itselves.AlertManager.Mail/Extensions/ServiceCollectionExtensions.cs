using System;
using Itselves.AlertManager.Abstraction.Extensions;
using Itselves.AlertManager.Mail.Environment;
using Itselves.AlertManager.Mail.Formatter;
using Itselves.AlertManager.Mail.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Itselves.AlertManager.Mail.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMailAlertManager(
        this IServiceCollection services,
        Action<MailAlertManagerOptions>? configure = null)
    {
        services
            .AddOptions<MailAlertManagerOptions>()
            .BindConfiguration(nameof(MailAlertManagerOptions))
            .Configure(opt => configure?.Invoke(opt))
            .Validate(opt => !string.IsNullOrEmpty(opt.Host), "MailAlertManagerOptions.Host is required.")
            .Validate(opt => !string.IsNullOrEmpty(opt.From), "MailAlertManagerOptions.From is required.")
            .Validate(opt => opt.To.Length > 0, "MailAlertManagerOptions.To must contain at least one recipient.")
            .Validate(
                validation: opt => !opt.UseDefaultCredentials || string.IsNullOrEmpty(opt.Username),
                failureMessage: "MailAlertManagerOptions.UseDefaultCredentials cannot be true when Username is provided.");

        services
            .AddAlertManager<MailAlertManager>();

        services
            .TryAddSingleton<IAlertFormatter, ReflectionFormatter>();

        services
            .TryAddSingleton<IDateTimeProvider, DefaultDateTimeProvider>();

        return services;
    }

    private sealed class DefaultDateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset GetNow() => DateTimeOffset.UtcNow;
    }
}
