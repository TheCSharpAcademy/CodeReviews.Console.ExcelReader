using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace ExcelReader.Data;

public class DapperContext(IConfiguration configuration, ILogger<DapperContext> logger)
{
    public IDbConnection GetConnection()
    {
        var connection = new SqliteConnection(configuration.GetConnectionString("DefaultConnection"));
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        return connection;
    }
    
    public void EnsureDeleted()
    {
        if (!File.Exists("reader.db")) return;
        
        logger.Log(LogLevel.Information,"Deleting database.");
        File.Delete("reader.db");
    }

    public void EnsureCreated()
    {
        logger.Log(LogLevel.Information,"Creating database.");

        using var connection = GetConnection();
        var sql = """
                  CREATE TABLE Gods (
                      Name TEXT not null,
                      Domain TEXT not null,
                      Symbol text not null,
                      Fame text not null
                  )
                  """;
        connection.Execute(sql);
    }
}