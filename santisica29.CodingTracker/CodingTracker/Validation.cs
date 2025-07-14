using System.Globalization;
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker;

internal static class Validation
{
    internal static bool IsFormattedCorrectly(string date, string format)
    {
        if (!DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _) || String.IsNullOrEmpty(date))
        {
            return false;
        }

        return true;
    }

    internal static bool IsEndTimeLowerThanStartTime(string startTime, string endTime)
    {
        var sT = DateTime.ParseExact(startTime, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"));

        var eT = DateTime.ParseExact(endTime, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"));

        return eT < sT;
    }

    internal static void IsListEmpty<T>(List<T>? list)
    {
        return list.Count == 0
    }
}
