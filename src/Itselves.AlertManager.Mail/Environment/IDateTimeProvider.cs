using System;

namespace Itselves.AlertManager.Mail.Environment;

internal interface IDateTimeProvider
{
    DateTimeOffset GetNow();
}
