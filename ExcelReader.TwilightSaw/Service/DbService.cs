using Dapper;
using ExcelReader.TwilightSaw.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Table = Spectre.Console.Table;

namespace ExcelReader.TwilightSaw.Service;

public class DbService(IConfiguration configuration, ReaderService readerService)
{
    private readonly SqlConnection _connection = new(configuration.GetConnectionString("DefaultConnection"));
    private readonly ReaderItem _file = readerService.ReadFormat();
    public async void CreateDb()
    {
        Console.WriteLine("Creating database...");
        await using var connection = _connection;
        var query = $"""
                     IF EXISTS (SELECT name FROM sys.databases WHERE name = '{_file.DbName}')
                                         BEGIN
                                         ALTER DATABASE {_file.DbName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                         DROP DATABASE {_file.DbName};
                                         END
                     CREATE DATABASE {_file.DbName}
                     """;
        connection.Execute(query);
        Console.WriteLine("Database created!");
    }

    public bool CreateTable()
    {
        Console.WriteLine("Creating tables...");
        try
        {
            foreach (var value in _file.Tables)
            {
                var data = value.table.Select(i => i.ToList()).ToList();
                for (var index = 0; index < data[0].Count; index++)
                {
                    data[0][index] = data[0][index].Replace(" ", "");
                    value.table[0][index] = data[0][index].Replace(" ", "");
                    data[0][index] += " TEXT";
                }

                var columns = string.Join(", ", data[0]);
                var copiedColumns = string.Join(", ", value.table[0]);
                var builder = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"))
                {
                    InitialCatalog = _file.DbName
                };
                var dbConnection = builder.ConnectionString;
                using var connection = new SqlConnection(dbConnection);

                connection.Execute($"CREATE TABLE {value.tableName} ({columns});");
                for (var index = 1; index < value.table.Count; index++)
                {
                    var cellsList = value.table[index].Select(t => $"'{t}'");
                    var values = string.Join(", ", cellsList);
                    var sql = @$"INSERT INTO {value.tableName} ({copiedColumns}) VALUES ({values});";
                    connection.Execute(sql);
                }
            }
            Console.WriteLine("Tables created!");
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    public async void Read()
    {
        Console.WriteLine("Fetching data from the database...");
        foreach (var value in _file.Tables)
        {
            var builder = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"))
            {
                InitialCatalog = _file.DbName
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
            foreach (var column in tablesColumns) table.AddColumn(column);
            foreach (var row in tablesRows) table.AddRow(row.ToArray());
            AnsiConsole.Write(table);
        }
    }
    public bool IsValid()
    {
        if (_file == default) return false;
        CreateDb();
        if(!CreateTable()) return false; 
        Read();
        return true;
    }
}