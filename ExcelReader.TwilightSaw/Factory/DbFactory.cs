using Dapper;
using ExcelReader.TwilightSaw.Controller;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ExcelReader.TwilightSaw.Factory;

public class DbFactory(IConfiguration configuration, ReaderController readerController)
{
    private readonly SqlConnection _connection = new (configuration.GetConnectionString("DefaultConnection"));
    public void CreateDb()
    {
        var data = readerController.Read();
        using var connection = _connection;
        var query = $"""
                     IF EXISTS (SELECT name FROM sys.databases WHERE name = '{data.name}')
                                         BEGIN
                                         DROP DATABASE {data.name};
                                         END
                     CREATE DATABASE {data.name}
                     """;
        connection.Execute(query);
    }

    public void CreateTable()
    {
        var data = readerController.Read();
        for (var index = 0; index < data.values[0].Count; index++)
        {
            var cell = data.values[0][index];
            data.values[0][index] = cell + " TEXT";
        }

        var columns = string.Join(", ", data.values[0]);
        var columns2 = string.Join(", ", readerController.Read().values[0]);

        var builder = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"))
        {
            InitialCatalog = data.name 
        };
        var dbConnection = builder.ConnectionString;
        using var connection = new SqlConnection(dbConnection);

        connection.Execute($"CREATE TABLE {data.name} ({columns});");
        for (var index = 1; index < data.values.Count; index++)
        {
            var t1 = data.values[index].Select(t => $"'{t}'");
            var values = string.Join(", ", t1);
            var sql = @$"INSERT INTO {data.name} ({columns2}) VALUES ({values});";
            connection.Execute(sql);
        }
    }
}