using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker.Data;
internal static class DatabaseInitializer
{
    internal static string GetConnectionString()
    {
        return ConfigurationManager.AppSettings["ConnectionString"];
    }

    internal static string GetDBName()
    {
        return ConfigurationManager.AppSettings["DatabasePath"];
    }
    internal static void CreateDatabase()
    {
        using var connection = new SqliteConnection(GetConnectionString());
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            @$"CREATE TABLE IF NOT EXISTS {GetDBName()}(
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            StartTime TEXT,
            EndTime TEXT,
            Duration TEXT
            )";

        tableCmd.ExecuteNonQuery();
    }
}
