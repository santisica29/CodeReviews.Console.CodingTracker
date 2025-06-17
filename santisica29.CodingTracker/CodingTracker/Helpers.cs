using CodingTracker.Models;
using CodingTracker.View;
using ConsoleTableExt;
using Spectre.Console;
using System.Diagnostics;
using System.Globalization;

namespace CodingTracker;
internal static class Helpers
{
    private static readonly UserInterface userInterface = new();

    internal static List<CodingSession> ParseAnonObjToCodingSession(List<dynamic> listFromDB)
    {
        var list = new List<CodingSession>();

        foreach (var item in listFromDB)
        {
            var newCodingSession = new CodingSession()
            {
                Id = (int)item.Id,
                StartTime = DateTime.ParseExact(item.StartTime, "yyyy-MM-dd HH:mm", new CultureInfo("en-US")),
                EndTime = DateTime.ParseExact(item.EndTime, "yyyy-MM-dd HH:mm", new CultureInfo("en-US")),
                Duration = TimeSpan.Parse(item.Duration),
            };


            list.Add(newCodingSession);
        }

        return list;
    }

    internal static string GetDateInput(string message)
    {
        var dateInput = AnsiConsole.Prompt(
            new TextPrompt<string>(message));

        if (dateInput.ToLower() == "t") return DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        while (!IsFormattedCorrectly(dateInput, "yyyy-MM-dd HH:mm") || String.IsNullOrEmpty(dateInput))
        {
            dateInput = AnsiConsole.Prompt(
            new TextPrompt<string>("Invalid input, try again."));
        }

        return dateInput;
    }

    internal static bool IsFormattedCorrectly(string date, string format)
    {
        if (!DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _))
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

    internal static string RunStopWatch()
    {
        AnsiConsole.MarkupLine(@"-------- TIME YOUR SESSION ---------
        Press 'q' to quit.
        Press 'p' to pause
        Press 'r' to reset
        ");

        Stopwatch stopwatch = new();
        stopwatch.Start();

        bool isRunning = true;
        bool isPaused = false;

        while (isRunning)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.P:
                        if (isPaused)
                        {
                            stopwatch.Start();
                            isPaused = false;
                        }
                        else
                        {
                            stopwatch.Stop();
                            isPaused = true;
                        }
                        break;

                    case ConsoleKey.Q:
                        isRunning = false;
                        stopwatch.Stop();
                        break;

                    case ConsoleKey.R:
                        stopwatch.Restart();
                        break;
                }
            }
            var time = stopwatch.Elapsed;
            Console.SetCursorPosition(0, Console.CursorTop);
            AnsiConsole.Markup($"Time: {time.ToString(@"hh\:mm\:ss")}");
            Thread.Sleep(50);
        }

        var endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        AnsiConsole.MarkupLine("\nSession finished.\nPress any key to continue.");
        Console.ReadKey();

        return endTime;
    }

    internal static void CreateTable(List<CodingSession> list, string[]? arr = null)
    {
        var tableData = new List<List<object>>();

        foreach (var item in list)
        {
            var row = new List<object>
            {
                item.Id,
                item.StartTime.ToString("dd-MMM-yyyy HH:mm tt"),
                item.EndTime.ToString("dd-MMM-yyyy HH:mm tt"),
                $"{item.Duration.Hours}h {item.Duration.Minutes}m"
            };

            tableData.Add(row);
        }

        ConsoleTableBuilder
            .From(tableData)
            .WithColumn(arr)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .WithTitle("Your report",ConsoleColor.DarkYellow)
            .ExportAndWriteLine();
    }

    internal static void CreateTableOfAvg(List<string> list)
    {
        var total = TimeSpan.Zero;
        var count = list.Count;

        foreach (var item in list)
        {
            total += TimeSpan.Parse(item);
        }

        var avg = total / count;

        var newObject = new List<object>()
        {
            new
            {
               NumOfSessions = count,
               Total = $"{total.Hours}h {total.Minutes}m",
               Avg = $"{avg.Hours}h {avg.Minutes}m"
            }
            
        };

        ConsoleTableBuilder
            .From(newObject)
            .WithColumn("Num of Sessions", "Total", "Average per day")
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .WithTitle("-------------")
            .ExportAndWriteLine();
    }

    internal static void CheckIfListIsNullOrEmpty(List<CodingSession>? list)
    {
        if (list == null || list.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No data found.[/]");
            AnsiConsole.MarkupLine("Press Any Key to Continue.");
            Console.ReadKey();
            return;
        }
    }
}
