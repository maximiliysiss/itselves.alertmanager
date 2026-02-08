using Microsoft.Extensions.Logging;

namespace Itselves.AlertManager.Mail.Options;

public sealed class MailAlertManagerOptions
{
    /// <summary>
    /// SMTP host.
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// SMTP port.
    /// </summary>
    public int Port { get; set; } = 25;

    /// <summary>
    /// Enable SSL/TLS for SMTP.
    /// </summary>
    public bool EnableSsl { get; set; }

    /// <summary>
    /// Use default credentials from the current process.
    /// </summary>
    public bool UseDefaultCredentials { get; set; }

    /// <summary>
    /// Optional SMTP username.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Optional SMTP password.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Sender email address.
    /// </summary>
    public string From { get; set; } = string.Empty;

    /// <summary>
    /// Recipient email addresses.
    /// </summary>
    public string[] To { get; set; } = [];

    /// <summary>
    /// Email subject template. Use {Alert} or {UtcNow}.
    /// </summary>
    public string Subject { get; set; } = "Alert: {Alert}";

    /// <summary>
    /// Email body template. Use {Alert} or {UtcNow}.
    /// </summary>
    public string Body { get; set; } = "Alert: {Alert}";

    /// <summary>
    /// Treat email body as HTML.
    /// </summary>
    public bool IsBodyHtml { get; set; }

    /// <summary>
    /// Log level for alerts.
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;
}
