using CodingTracker.Data;
using CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using static CodingTracker.Enums;
using System.Globalization;

namespace CodingTracker.Controller;
internal class CodingController
{
    private readonly DatabaseMethods databaseMethods = new();

    public void AddSession(string? startTime = null, string? endTime = null)
    {
        if (startTime == null || endTime == null)
        {
            startTime = Helpers.GetDateInput("Enter the start time of your coding session (yyyy-MM-dd HH:mm).\nPress 't' to enter actual time.");

            endTime = Helpers.GetDateInput("Enter the end time of your coding session (yyyy-MM-dd HH:mm)\nPress 't' to enter actual time.");
        }

        while (Helpers.IsEndTimeLowerThanStartTime(startTime, endTime))
        {
            endTime = Helpers.GetDateInput("Invalid input. End time must be larger than the start time.");
        }

        var session = new CodingSession(
            DateTime.ParseExact(startTime, "yyyy-MM-dd HH:mm", new CultureInfo("en-US")),
            DateTime.ParseExact(endTime, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"))
        );

        var newSession = new { StartTime = startTime, EndTime = endTime, Duration = session.CalculateDuration().ToString() };

        var affectedRows = databaseMethods.CreateSession(newSession);

        if (affectedRows > 0) Helpers.DisplayMessage("Addition completed.", "green");
        else Helpers.DisplayMessage("No changes made");

        AnsiConsole.MarkupLine("Press any key to continue.");
        Console.ReadKey();
    }

    public void DeleteSession()
    {
        var list = GetSessions();

        Helpers.CheckIfListIsNullOrEmpty(list);

        var sessionToDelete = AnsiConsole.Prompt(
            new SelectionPrompt<CodingSession>()
            .Title("Select a [red]session[/] to delete:")
            .UseConverter(s => $"{s.Id} - {s.StartTime} - {s.EndTime} - {s.Duration}")
            .AddChoices(list));

        if (Helpers.ConfirmDeletion(sessionToDelete.ToString()))
        {
            var affectedRows = databaseMethods.DeleteSession(sessionToDelete);

            if (affectedRows > 0) Helpers.DisplayMessage("Deletion completed.", "green");
            else Helpers.DisplayMessage("No changes made");
        }
        else
        {
            Helpers.DisplayMessage("Deletion canceled", "yellow");
        }

        AnsiConsole.MarkupLine("Press any key to continue.");
        Console.ReadKey();
    }

    public void UpdateSession()
    {
        var list = GetSessions();

        Helpers.CheckIfListIsNullOrEmpty(list);

        var sessionToUpdate = AnsiConsole.Prompt(
            new SelectionPrompt<CodingSession>()
            .Title("Select a [red]session[/] to update:")
            .UseConverter(s => $"{s.Id} - {s.StartTime} - {s.EndTime} - {s.Duration}")
            .AddChoices(list));

        var newStartTime = Helpers.GetDateInput("Enter the start time of your coding session (yyyy-MM-dd HH:mm)");
        var newEndTime = Helpers.GetDateInput("Enter the end time of your coding session (yyyy-MM-dd HH:mm)");

        while (Helpers.IsEndTimeLowerThanStartTime(newStartTime, newEndTime))
        {
            newEndTime = Helpers.GetDateInput("Invalid input. End time must be higher than start time.");
        }

        var session = new CodingSession(
            DateTime.ParseExact(newStartTime, "yyyy-MM-dd HH:mm", new CultureInfo("en-US")),
            DateTime.ParseExact(newEndTime, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"))
        );

        var newSession = new { Id = sessionToUpdate.Id, NewStartTime = newStartTime, NewEndTime = newEndTime, Duration = session.CalculateDuration().ToString()};

        var affectedRows = databaseMethods.UpdateSession(newSession);

        if (affectedRows > 0) Helpers.DisplayMessage("Update successful.", "green");
        else Helpers.DisplayMessage("No changes made");

        AnsiConsole.MarkupLine("Press any key to continue.");
        Console.ReadKey();
    }
    public void StartSession()
    {
        Helpers.DisplayMessage("Session started", "blue");
        var startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        var timer = Helpers.RunStopWatch();

        var choice = AnsiConsole.Confirm("Do you want to save your session?");

        if (!choice) return;

        var endTime = timer;

        AddSession(startTime, endTime);
    }

    public List<CodingSession>? GetSessions(string? sql = null)
    {
        using var connection = new SqliteConnection(DatabaseInitializer.GetConnectionString());

        if (sql == null) sql = $"SELECT * FROM coding_tracker";

        var listFromDB = connection.Query(sql).ToList();

        if (listFromDB.Count == 0) return null;

        var listOfCodingSessions = Helpers.ParseAnonObjToCodingSession(listFromDB);

        return listOfCodingSessions;
    }

    public void ViewSessions(List<CodingSession>? list = null, List<string> additionalList = null)
    {
        Helpers.CheckIfListIsNullOrEmpty(list);

        Helpers.CreateTable(list, ["ID", "Start Time", "End Time", "Duration"]);

        if (additionalList != null)
        {
            Helpers.CreateTableOfAvg(additionalList);
        }

        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    public void ViewReportOfCodingSession()
    {
        Helpers.DisplayMessage("Get your coding tracker report!");

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<ReportOption>()
            .Title("Select your report type")
            .AddChoices(Enum.GetValues<ReportOption>())
            );

        string? unit = null;

        if (choice != ReportOption.Total)
        {
            unit = AnsiConsole.Prompt(
                new TextPrompt<string>($"Select the number of {choice} for your report."));
        }

        var listOfReport = GetReport(choice, unit);

        var additionalList = GetReportOfTotalAndAvg(choice, unit);

        ViewSessions(listOfReport, additionalList);
    }

    public List<CodingSession>? GetReport(ReportOption choice, string? unit)
    {
        var sql = $"SELECT * FROM coding_tracker ";

        _ = choice switch
        {
            ReportOption.Days => sql += $"WHERE EndTime > date('now', '-{unit} days')",
            ReportOption.Months => sql += $"WHERE EndTime > date('now','start of month', '-{unit} months')",
            ReportOption.Years => sql += $"WHERE EndTime > date('now','start of year', '-{unit} years')",
            ReportOption.Total => sql
        };

        sql += " ORDER BY StartTime DESC";

        return GetSessions(sql);
    }

    public List<string>? GetReportOfTotalAndAvg(ReportOption choice, string? unit)
    {
        using var connection = new SqliteConnection(DatabaseInitializer.GetConnectionString());

        var sql = $"SELECT Duration FROM coding_tracker ";
        _ = choice switch
        {
            ReportOption.Days => sql += $"WHERE EndTime > date('now', '-{unit} days')",
            ReportOption.Months => sql += $"WHERE EndTime > date('now','start of month', '-{unit} months')",
            ReportOption.Years => sql += $"WHERE EndTime > date('now','start of year', '-{unit} years')",
            ReportOption.Total => sql
        };

        sql += " ORDER BY StartTime DESC";

        var list = connection.Query<string>(sql).ToList();

        return list;
    }  
}
