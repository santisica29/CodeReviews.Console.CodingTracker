using CodingTracker.Models;
using static CodingTracker.Enums;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace CodingTracker.Data;

internal class DatabaseMethods
{
    public int CreateSession(object newSession)
    {
        using var connection = new SqliteConnection(DatabaseInitializer.GetConnectionString());

        var sql =
            @$"INSERT INTO {DatabaseInitializer.GetDBName()} (startTime, endTime, duration)
               VALUES (@StartTime, @EndTime, @Duration)";

        var affectedRows = connection.Execute(sql, newSession);

        return affectedRows;
    }

    public int DeleteSession(CodingSession sessionToDelete)
    {
        using var connection = new SqliteConnection(DatabaseInitializer.GetConnectionString());
        var sql = $@"DELETE from {DatabaseInitializer.GetDBName()} WHERE Id = @Id";

        var affectedRows = connection.Execute(sql, new { sessionToDelete.Id });

        return affectedRows;
    }

    public int UpdateSession(object session)
    {
        using var connection = new SqliteConnection(DatabaseInitializer.GetConnectionString());

        var sql = @$"UPDATE {DatabaseInitializer.GetDBName()} 
                    SET StartTime = @NewStartTime, 
                    EndTime = @NewEndTime,
                    Duration = @Duration
                    WHERE Id = @Id";

        var affectedRows = connection.Execute(sql, session);

        return affectedRows;
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
