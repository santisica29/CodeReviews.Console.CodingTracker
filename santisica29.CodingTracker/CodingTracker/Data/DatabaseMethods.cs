using CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Data;
internal class DatabaseMethods
{
    public int CreateSession(CodingSession newSession)
    {
        using var connection = new SqliteConnection(DatabaseInitializer.GetConnectionString());

        var sql =
            @$"INSERT INTO {DatabaseInitializer.GetDBName()} (startTime, endTime, duration)
               VALUES (@StartTime, @EndTime, @Duration)";

        var affectedRows = connection.Execute(sql, new { StartTime = newSession.StartTime, EndTime = newSession.EndTime, Duration = newSession.CalculateDuration().ToString()});

        return affectedRows;
    }
}
