using System;
using Itselves.AlertManager.Abstraction.Extensions;
using Itselves.AlertManager.Telegram.Environment;
using Itselves.AlertManager.Telegram.Formatter;
using Itselves.AlertManager.Telegram.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Itselves.AlertManager.Telegram.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramAlertManager(
        this IServiceCollection services,
        Action<TelegramAlertManagerOptions>? configure = null)
    {
        services
            .AddOptions<TelegramAlertManagerOptions>()
            .BindConfiguration(nameof(TelegramAlertManagerOptions))
            .Configure(opt => configure?.Invoke(opt))
            .Validate(opt => !string.IsNullOrEmpty(opt.BotToken), "TelegramAlertManagerOptions.BotToken is required.")
            .Validate(opt => opt.ChatIds.Length > 0, "TelegramAlertManagerOptions.ChatIds must contain at least one chat ID.");

        services
            .AddAlertManager<TelegramAlertManager>();

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
