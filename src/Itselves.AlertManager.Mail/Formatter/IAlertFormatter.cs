using Itselves.AlertManager.Abstraction.Models;

namespace Itselves.AlertManager.Mail.Formatter;

internal interface IAlertFormatter
{
    public string Format(string message, Alert alert);
}
