using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Table = Spectre.Console.Table;

namespace ExcelReader.TwilightSaw.Controller;

public class DbController(IConfiguration configuration, ReaderController readerController)
{
    private readonly SqlConnection _connection = new(configuration.GetConnectionString("DefaultConnection"));
    public void CreateDb()
    {
        var data = readerController.Read();
        using var connection = _connection;
        var query = $"""
                     IF EXISTS (SELECT name FROM sys.databases WHERE name = '{data.name}')
                                         BEGIN
                                         ALTER DATABASE {data.name} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                         DROP DATABASE {data.name};
                                         END
                     CREATE DATABASE {data.name}
                     """;
        connection.Execute(query);
    }

    public void CreateTable(out string name)
    {
        var data = readerController.Read();
        name = data.name;
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

    public void Read()
    {
        var builder = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"))
        {
            InitialCatalog = readerController.Read().name
        };
        using var connection = new SqlConnection(builder.ConnectionString);
        var result = connection.Query($"SELECT * FROM {readerController.Read().name}").Select(r => (IDictionary<string, object>)r).ToList();

        var tablesColumns = new List<string>();
        var tablesRows = result.Select(pairs => 
            pairs.Select(pair => 
                pair.Value.ToString()).
                ToList()).
            ToList();

        tablesColumns.AddRange(result[0].Keys);

        var table = new Table();
        foreach (var r in tablesColumns) table.AddColumn(r);
        foreach (var r in tablesRows) table.AddRow(r.ToArray());
        AnsiConsole.Write(table);
    }
}