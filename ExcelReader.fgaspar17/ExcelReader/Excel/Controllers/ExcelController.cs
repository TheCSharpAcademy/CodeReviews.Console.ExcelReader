using Dapper;
using Microsoft.Data.Sqlite;

namespace ExcelReader;

public class ExcelController
{
    public static bool InsertData(string tableName, Dictionary<string, string> columnValue, string connStr)
    {
        var dictionary = new Dictionary<string, object>();
        foreach (var column in columnValue)
        {
            dictionary.Add($"@{column.Key}", column.Value);
        }

        var parameters = new DynamicParameters(dictionary);

        string sql = $@"INSERT INTO ""{tableName}"" ({string.Join(", ", columnValue.Keys)}) 
                                        VALUES ({string.Join(", ", columnValue.Keys.Select(c => $"@{c}"))});";

        return SqlExecutionService.ExecuteCommand(sql, parameters, connStr);
    }

    public static List<string> GetTables(string connStr)
    {
        string sql = "SELECT name FROM sqlite_master WHERE type='table';";

        try
        {
            using SqliteConnection connection = new SqliteConnection(connStr);
            var tables = connection.Query<string>(sql).ToList();
            return tables;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error retrieving tables: " + ex.Message);
            return new List<string>();
        }
    }

    public static List<Dictionary<string, string>> GetDataFromTable(string table, string connStr)
    {
        string sql = $@"SELECT * FROM ""{table}"" ";

        try
        {
            using SqliteConnection connection = new SqliteConnection(connStr);
            var dynamicRows = connection.Query(sql).ToList();

            List<IDictionary<string, object>> dictionaryData = dynamicRows.Select(x => (IDictionary<string, object>)x).ToList();

            return dictionaryData
                .Select(dict => dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? string.Empty))
                .ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving data from table {table}: " + ex.Message);
            return new List<Dictionary<string, string>>();
        }
    }

    public static List<string> GetColumnsFromTable(string table, string connStr)
    {
        string sql = $@"SELECT NAME FROM pragma_table_info(""{table}"") ;";

        try
        {
            using SqliteConnection connection = new SqliteConnection(connStr);
            return connection.Query<string>(sql).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving columns from table {table}: " + ex.Message);
            return new List<string>();
        }
    }
}