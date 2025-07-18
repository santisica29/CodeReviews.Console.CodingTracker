﻿using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker.Data;
internal static class DatabaseInitializer
{
    internal static string GetConnectionString()
    {
        string dbPath = Path.Combine(ProjectRoot(), GetDBName());
        string connectionString = $"Data Source={dbPath}.db";
        return connectionString;
    }

    internal static string GetDBName()
    {
        return ConfigurationManager.AppSettings["DatabaseFileName"];
    }

    internal static string ProjectRoot()
    {
        return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
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
