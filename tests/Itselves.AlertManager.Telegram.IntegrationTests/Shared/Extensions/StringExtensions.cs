using System;
using System.Text;

namespace Itselves.AlertManager.Telegram.IntegrationTests.Shared.Extensions;

internal static class StringExtensions
{
    public static Guid AsGuid(this string value)
    {
        const int guidLength = 16;

        while (value.Length < guidLength)
            value += value;

        return new Guid(Encoding.UTF8.GetBytes(value[..16]));
    }
}
