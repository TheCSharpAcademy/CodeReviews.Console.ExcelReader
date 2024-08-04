using System.Data;
using ExcelReader.kwm0304.Models;
using Microsoft.Data.SqlClient;

namespace ExcelReader.kwm0304.Data;

public class DatabaseAccess
{
  private readonly string _connectionString;

  public DatabaseAccess(string connectionString)
  {
    _connectionString = connectionString;
  }

  public void SaveToDatabase(Response<Dictionary<string, object>> response)
  {
    using var connection = new SqlConnection(_connectionString);
    connection.Open();
        CreateTableIfNotExists(connection, response.ColumnNames);
    var insertCommand = PrepareInsertCommand(connection, response.ColumnNames);
    foreach (var row in response.RowValues)
    {
      foreach (var column in response.ColumnNames)
      {
        insertCommand.Parameters[$"@{column}"].Value = row[column] ?? DBNull.Value;
      }
      insertCommand.ExecuteNonQuery();
    }
  }

  private static void CreateTableIfNotExists(SqlConnection connection, List<string> columnNames)
  {
    var columnDefinitions = columnNames.Select(col => $"[{col}] NVARCHAR(255)");
    var createTableQuery = $@"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='csvReaderDb' AND xtype='U')
            CREATE TABLE csvReaderDb (
                Id INT PRIMARY KEY IDENTITY(1,1),
                {string.Join(",\n", columnDefinitions)}
            )";
    using var command = new SqlCommand(createTableQuery, connection);
    command.ExecuteNonQuery();
  }

  private static SqlCommand PrepareInsertCommand(SqlConnection connection, List<string> columnNames)
  {
    var columns = string.Join(", ", columnNames.Select(col => $"[{col}]"));
    var parameters = string.Join(", ", columnNames.Select(col => $"@{col}"));
    var insertQuery = $"INSERT INTO csvReaderDb ({columns}) VALUES ({parameters})";

    var command = new SqlCommand(insertQuery, connection);
    foreach (var column in columnNames)
    {
      command.Parameters.Add($"@{column}", SqlDbType.NVarChar);
    }
    return command;
  }
}