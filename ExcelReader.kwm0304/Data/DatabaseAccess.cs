using System.Data;
using System.Text;
using ExcelReader.kwm0304.Models;
using ExcelReader.kwm0304.Utils;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace ExcelReader.kwm0304.Data;

public class DatabaseAccess(string connectionString, string tableName = "csvReaderDb")
{
  private readonly string _connectionString = connectionString;
  private readonly string _tableName = tableName;
  private readonly Validation _validation = new();

  public void DropAllTables()
  {
    using var connection = new SqlConnection(_connectionString);
    connection.Open();
    string dropTablesScript = GenerateDropTablesScript(connection);
    if (string.IsNullOrEmpty(dropTablesScript))
    {
      AnsiConsole.WriteLine("No tables found in the database. Continuing...");
      return;
    }
    ExecuteScript(connection, dropTablesScript);
    AnsiConsole.WriteLine("All tables in the database have been dropped.");
  }

  static string GenerateDropTablesScript(SqlConnection connection)
  {
    StringBuilder sb = new();
    string query = @"
            SELECT 'DROP TABLE ' + QUOTENAME(SCHEMA_NAME(schema_id)) + '.' + QUOTENAME(name) + ';'
            FROM sys.tables
            WHERE type = 'U';";

    using (SqlCommand command = new(query, connection))
    {
      using SqlDataReader reader = command.ExecuteReader();
      while (reader.Read())
      {
        sb.AppendLine(reader.GetString(0));
      }
    }
    return sb.ToString();
  }

  static void ExecuteScript(SqlConnection connection, string script)
  {
    using SqlCommand command = new(script, connection);
    command.ExecuteNonQuery();
  }

  public void SaveToDatabase(Response<List<Dictionary<string, object>>> response)
  {
    using var connection = new SqlConnection(_connectionString);
    connection.Open();
    CreateTableIfNotExists(connection, response.ColumnNames);
    var insertCommand = PrepareInsertCommand(connection, response.ColumnNames);
    for (int rowIndex = 0; rowIndex < response.RowValues.Count; rowIndex++)
    {
      var row = response.RowValues[rowIndex];
      if (row.Values.All(value => value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString())))
      {
        AnsiConsole.WriteLine($"Skipping empty row at index {rowIndex}");
        continue;
      }
      insertCommand.Parameters.Clear();
      for (int i = 0; i < response.ColumnNames.Count; i++)
      {
        var columnName = response.ColumnNames[i];
        var paramName = $"@{_validation.NormalizeColumnName(columnName)}";
        var value = row.TryGetValue(columnName, out object? val) ? val : DBNull.Value;
        SqlParameter parameter = new SqlParameter(paramName, _validation.GetSqlDbType(value))
        {
          Value = value
        };
        insertCommand.Parameters.Add(parameter);
      }
      insertCommand.ExecuteNonQuery();
    }
  }

  private void CreateTableIfNotExists(SqlConnection connection, List<string> columnNames)
  {
    var columnDefinitions = columnNames.Select(col => $"[{_validation.SanitizeIdentifier(col)}] NVARCHAR(MAX)").ToList();
    var createTableQuery = $@"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{_tableName}' AND xtype='U')
            CREATE TABLE [{_tableName}] (
                Id INT PRIMARY KEY IDENTITY(1,1),
                {string.Join(", ", columnDefinitions)}
            )";
    using var command = new SqlCommand(createTableQuery, connection);
    command.ExecuteNonQuery();
  }

  private SqlCommand PrepareInsertCommand(SqlConnection connection, List<string> columnNames)
  {
    var columns = string.Join(", ", columnNames.Select(col => $"[{_validation.SanitizeIdentifier(col)}]"));
    var parameters = string.Join(", ", columnNames.Select(col => $"@{col}"));
    var insertQuery = $"INSERT INTO [{_tableName}] ({columns}) VALUES ({parameters})";
    var command = new SqlCommand(insertQuery, connection);
    foreach (var column in columnNames)
    {
      command.Parameters.Add(new SqlParameter($"@{column}", SqlDbType.NVarChar, -1));
    }
    return command;
  }
}