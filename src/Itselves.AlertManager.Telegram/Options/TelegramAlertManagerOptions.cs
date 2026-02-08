using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.Enums;

namespace Itselves.AlertManager.Telegram.Options;

public sealed class TelegramAlertManagerOptions
{
    /// <summary>
    /// Base URL for the Telegram Bot API server.
    /// Useful for custom or self-hosted instances of Telegram Bot API.
    /// </summary>
    public string? BaseUrl { get; set; }

    /// <summary>
    /// Telegram bot token.
    /// </summary>
    public string BotToken { get; set; } = string.Empty;

    /// <summary>
    /// Telegram chat IDs to send alerts to.
    /// </summary>
    public string[] ChatIds { get; set; } = [];

    /// <summary>
    /// Message template. Use {Alert} or {UtcNow}.
    /// </summary>
    public string Message { get; set; } = "Alert: {Alert}";

    /// <summary>
    /// Telegram parse mode. Examples: MarkdownV2, HTML.
    /// </summary>
    public ParseMode ParseMode { get; set; } = ParseMode.None;

    /// <summary>
    /// Disable notification sound.
    /// </summary>
    public bool DisableNotification { get; set; }

    /// <summary>
    /// Log level for alerts.
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;
}
