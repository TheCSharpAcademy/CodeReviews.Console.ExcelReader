using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace ExcelReader;

public class SqlExecutionService
{
    public static bool ExecuteCommand(string sql, DynamicParameters parameters, string connStr)
    {
        try
        {
            using (IDbConnection connection = new SqliteConnection(connStr))
            {
                connection.Open();

                connection.Execute(sql, parameters);

                connection.Close();
            }

            return true;
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error ocurred: {ex.Message}");
            return false;
        }
    }
}