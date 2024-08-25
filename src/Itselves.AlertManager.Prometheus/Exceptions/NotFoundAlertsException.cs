using System;

namespace Itselves.AlertManager.Prometheus.Exceptions;

public sealed class NotFoundAlertsException() : Exception("Not found any registered alerts");
