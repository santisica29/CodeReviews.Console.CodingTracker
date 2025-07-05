using CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;

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
}
