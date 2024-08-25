using System;

namespace Itselves.AlertManager.Prometheus.Environment;

internal interface IDateTimeProvider
{
    DateTimeOffset GetNow();
}
