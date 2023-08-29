using Microsoft.Data.SqlClient;

namespace ExcelReaderCarDioLogic;
internal class DatabaseLogic
{
    public void CreateTableAddData(List<string> headers, List<List<string>> dataRows)
    {
        Console.WriteLine("Creating database and table...");
        Thread.Sleep(1000);

        string tableName = "ExcelData";
        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ExcelReader;Trusted_Connection=True";
        string createTableQuery = $"CREATE TABLE {tableName} (";
        string insertDataQuery = $"INSERT INTO {tableName} (";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            foreach (var header in headers)
            {
                createTableQuery += $"{header} NVARCHAR(MAX), ";
                insertDataQuery += $"{header}, ";
            }

            createTableQuery = createTableQuery.TrimEnd(',', ' ') + ")";
            insertDataQuery = insertDataQuery.TrimEnd(',', ' ') + ") VALUES (";

            using (SqlCommand command = new SqlCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine($"Table '{tableName}' created successfully.");
            }

            foreach(var row in dataRows)
            {
                string insertDataQueryFinal = insertDataQuery;

                foreach (var cell in row)
                {
                    insertDataQueryFinal += $"'{cell}', ";
                }

                insertDataQueryFinal = insertDataQueryFinal.TrimEnd(',', ' ') + ")";

                using (SqlCommand command = new SqlCommand(insertDataQueryFinal, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        Console.WriteLine("Database created! Press any key to conclude!");
        Console.ReadLine();
    }

    public void CreateDatabase()
    {
        string databaseName = "excelReader";
        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Trusted_Connection=True";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{databaseName}') CREATE DATABASE {databaseName}";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery ();
            }

            connection.Close();
        }
    }

    public void DeleteDatabase()
    {
        string databaseName = "ExcelReader";
        string connectionString = "Server=(localdb)\\MSSQLLocalDB; Trusted_Connection=True";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string deleteQuery = $"ALTER DATABASE {databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE {databaseName}";

            using (SqlCommand command = new SqlCommand(deleteQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close ();
        }
    }
}
