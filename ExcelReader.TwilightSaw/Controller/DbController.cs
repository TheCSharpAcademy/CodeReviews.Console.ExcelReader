using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using static OfficeOpenXml.ExcelErrorValue;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Table = Spectre.Console.Table;

namespace ExcelReader.TwilightSaw.Controller;

public class DbController(IConfiguration configuration, ReaderController readerController)
{
    private readonly SqlConnection _connection = new(configuration.GetConnectionString("DefaultConnection"));
    private readonly (List<(List<List<string>> table, string tableName)> tables, string dbName) _excelFile = readerController.Read();
    public async void CreateDb()
    {
        Console.WriteLine("Creating database...");
        await using var connection = _connection;
        var query = $"""
                     IF EXISTS (SELECT name FROM sys.databases WHERE name = '{_excelFile.dbName}')
                                         BEGIN
                                         ALTER DATABASE {_excelFile.dbName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                         DROP DATABASE {_excelFile.dbName};
                                         END
                     CREATE DATABASE {_excelFile.dbName}
                     """;
        connection.Execute(query);
        Console.WriteLine("Database created!");
    }

    public async void CreateTable()
    {
        Console.WriteLine("Creating tables...");
        foreach (var value in _excelFile.tables)
        {
            var data = value.table.Select(i => i.ToList()).ToList();
            for (var index = 0; index < data[0].Count; index++) data[0][index] += " TEXT";

            var columns = string.Join(", ", data[0]);
            var columns2 = string.Join(", ", value.table[0]);

            var builder = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"))
            {
                InitialCatalog = _excelFile.dbName
            };
            var dbConnection = builder.ConnectionString;
            await using var connection = new SqlConnection(dbConnection);

            connection.Execute($"CREATE TABLE {value.tableName} ({columns});");
            for (var index = 1; index < value.table.Count; index++)
            {
                var t1 = value.table[index].Select(t => $"'{t}'");
                var values = string.Join(", ", t1);
                var sql = @$"INSERT INTO {value.tableName} ({columns2}) VALUES ({values});";
                connection.Execute(sql);
            }
        }
        Console.WriteLine("Tables created!");
    }

    public async void Read()
    {
        Console.WriteLine("Fetching data from the database...");
        foreach (var value in _excelFile.tables)
        {
            var builder = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"))
            {
                InitialCatalog = _excelFile.dbName
            };

            await using var connection = new SqlConnection(builder.ConnectionString);
            var result = connection.QueryAsync($"SELECT * FROM {value.tableName}").
                Result.Select(r => (IDictionary<string, object>)r).ToList();

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
}