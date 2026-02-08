using System.Threading;
using System.Threading.Tasks;
using Itselves.AlertManager.Abstraction;
using Itselves.AlertManager.Abstraction.Models;
using Itselves.AlertManager.Telegram.Formatter;
using Itselves.AlertManager.Telegram.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Itselves.AlertManager.Telegram;

internal sealed class TelegramAlertManager : IAlertManager
{
    private readonly ITelegramBotClient _client;

    private readonly ILogger<TelegramAlertManager> _logger;

    private readonly IAlertFormatter _formatter;

    private readonly TelegramAlertManagerOptions _options;

    public TelegramAlertManager(
        ILogger<TelegramAlertManager> logger,
        IOptions<TelegramAlertManagerOptions> options,
        IAlertFormatter formatter)
    {
        _logger = logger;
        _formatter = formatter;
        _options = options.Value;
        _client = new TelegramBotClient(new TelegramBotClientOptions(_options.BotToken, _options.BaseUrl));
    }

    public async Task AlertAsync(Alert alert, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.Log(_options.LogLevel, "Alert: {Alert}", alert);

        var message = _formatter.Format(_options.Message, alert);

        foreach (var chatId in _options.ChatIds)
            await SendMessageAsync(chatId, message, cancellationToken);
    }

    private Task SendMessageAsync(string chatId, string message, CancellationToken cancellationToken)
    {
        return _client.SendMessage(
            chatId: chatId,
            text: message,
            parseMode: _options.ParseMode,
            disableNotification: _options.DisableNotification,
            cancellationToken: cancellationToken);
    }
}
