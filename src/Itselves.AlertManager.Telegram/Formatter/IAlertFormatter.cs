using Itselves.AlertManager.Abstraction.Models;

namespace Itselves.AlertManager.Telegram.Formatter;

internal interface IAlertFormatter
{
    string Format(string message, Alert alert);
}
