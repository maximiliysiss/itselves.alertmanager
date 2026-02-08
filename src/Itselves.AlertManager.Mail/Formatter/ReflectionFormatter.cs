using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Itselves.AlertManager.Abstraction.Models;
using Itselves.AlertManager.Mail.Environment;

namespace Itselves.AlertManager.Mail.Formatter;

internal sealed class ReflectionFormatter(IDateTimeProvider dateTimeProvider) : IAlertFormatter
{
    private readonly ConcurrentDictionary<Type, Dictionary<string, Func<Alert, string>>> _formatters = [];

    public string Format(string message, Alert alert)
    {
        message = message
            .Replace("{Alert}", alert.ToString())
            .Replace("{UtcNow}", dateTimeProvider.GetNow().ToString());

        return _formatters
            .GetOrAdd(alert.GetType(), CreateFormatter)
            .Aggregate(message, (c, f) => c.Replace(f.Key, f.Value(alert)));

        static Dictionary<string, Func<Alert, string>> CreateFormatter(Type alertType)
        {
            return alertType
                .GetProperties()
                .Where(c => c.CanRead)
                .ToDictionary(c => $"{{{c.Name}}}", c => CreateGet(c.GetMethod));
        }

        static Func<Alert, string> CreateGet(MethodInfo method) => a => method.Invoke(a, []).ToString();
    }
}
