using CodingTracker.Models;
using CodingTracker.View;
using ConsoleTableExt;
using Spectre.Console;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace CodingTracker;
internal static class Helpers
{
    private static readonly UserInterface userInterface = new();

    internal static string GetDateInput(string message)
    {
        var dateInput = AnsiConsole.Prompt(
            new TextPrompt<string>(message));

        if (dateInput.ToLower() == "t") return DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        while (!Validation.IsFormattedCorrectly(dateInput, "yyyy-MM-dd HH:mm"))
        {
            dateInput = AnsiConsole.Prompt(
            new TextPrompt<string>("Invalid input, try again."));
        }

        return dateInput;
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
                $"{(int)item.Duration.TotalHours}h {item.Duration.Minutes}m"
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
               Total = $"{(int)total.TotalHours}h {total.Minutes}m",
               Avg = $"{(int)avg.TotalHours}h {avg.Minutes}m"
            }
            
        };

        ConsoleTableBuilder
            .From(newObject)
            .WithColumn("Num of Sessions", "Total", "Average per day")
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .WithTitle("-------------")
            .ExportAndWriteLine();
    }

    internal static void DisplayMessage(string message, string color = "blue")
    {
        AnsiConsole.MarkupLine($"[{color}]{message}[/]");
    }

    internal static bool ConfirmDeletion(string itemName)
    {
        var confrim = AnsiConsole.Confirm($"Are you sure you want to delete {itemName}?");

        return confrim;
    }

    internal static string GetDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        var attr = field?.GetCustomAttribute<DescriptionAttribute>();

        return attr?.Description ?? value.ToString();
    }
}
