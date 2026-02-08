using System;

namespace Itselves.AlertManager.Telegram.Environment;

internal interface IDateTimeProvider
{
    DateTimeOffset GetNow();
}
