using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Itselves.AlertManager.Abstraction;
using Itselves.AlertManager.Abstraction.Models;
using Itselves.AlertManager.Mail.Formatter;
using Itselves.AlertManager.Mail.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Itselves.AlertManager.Mail;

internal sealed class MailAlertManager : IAlertManager
{
    private readonly ILogger<MailAlertManager> _logger;

    private readonly IAlertFormatter _formatter;

    private readonly MailAlertManagerOptions _options;

    public MailAlertManager(ILogger<MailAlertManager> logger, IOptions<MailAlertManagerOptions> options, IAlertFormatter formatter)
    {
        _logger = logger;
        _formatter = formatter;
        _options = options.Value;
    }

    public async Task AlertAsync(Alert alert, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.Log(_options.LogLevel, "Alert: {Alert}", alert);

        using var message = new MailMessage
        {
            From = new MailAddress(_options.From),
            Subject = _formatter.Format(_options.Subject, alert),
            Body = _formatter.Format(_options.Body, alert),
            IsBodyHtml = _options.IsBodyHtml
        };

        foreach (var recipient in _options.To)
            message.To.Add(recipient);

        using var client = new SmtpClient(_options.Host, _options.Port)
        {
            EnableSsl = _options.EnableSsl,
            UseDefaultCredentials = _options.UseDefaultCredentials
        };

        if (!string.IsNullOrWhiteSpace(_options.Username))
            client.Credentials = new NetworkCredential(_options.Username, _options.Password ?? string.Empty);

        await client.SendMailAsync(message);
    }
}
