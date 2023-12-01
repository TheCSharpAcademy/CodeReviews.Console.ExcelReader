using Microsoft.Data.SqlClient;
using System.Data;

partial class Program
{
    static void CreateTable( string connectionString, string databaseName, DataTable excelSchema )
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"CREATE TABLE {databaseName}.dbo.FileRead (" +
                                      string.Join(", ", excelSchema.Columns.Cast<DataColumn>().Select(c => $"{ConvertColumnNames(c.ColumnName)} NVARCHAR(MAX)")) +
                                      ")";
                var response = command.ExecuteNonQuery();
            }
        }
    }

    static string ConvertColumnNames( string names )
    {
        return names.Replace(" ", "_").Trim();
    }

    static void SaveDataToDatabase( DataTable dt, string connectionString )
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            foreach (DataRow row in dt.Rows)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    // Assuming your table name is "YourTableName"
                    command.CommandText = $"INSERT INTO ReadFile ({string.Join(", ", dt.Columns.Cast<DataColumn>().Select(c => ConvertColumnNames(c.ColumnName)))}) VALUES ({string.Join(", ", dt.Columns.Cast<DataColumn>().Select(c => $"@{ConvertColumnNames(c.ColumnName)}"))})";

                    // Add parameters dynamically
                    foreach (DataColumn col in dt.Columns)
                    {
                        command.Parameters.AddWithValue($"@{col.ColumnName}", row[col]);
                    }

                    // Execute the query
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    static void CreateDatabase( string connectionString, string databaseName )
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string createDatabaseSql = $"CREATE DATABASE [{databaseName}]";

            using (SqlCommand command = new SqlCommand(createDatabaseSql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    static void DropDatabaseIfExists( string connectionString, string databaseName )
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string dropDatabaseSql = $"IF EXISTS (SELECT 1 FROM sys.databases WHERE name = '{databaseName}') " +
                                    $"DROP DATABASE [{databaseName}]";

            using (SqlCommand command = new SqlCommand(dropDatabaseSql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
