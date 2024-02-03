using Dapper;
using Microsoft.Data.SqlClient;
using System.Dynamic;

namespace ExcelReader.Doc415;

internal class DbHandler
{
    private string _connectionStringToDbUpdate = "Server=(localdb)\\MSSQLLocalDB; Integrated Security=true;";
    private string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=exceltodb; Integrated Security=true;";
    public void PrepareDatabase()
    {
        using (var connection = new SqlConnection(_connectionStringToDbUpdate))
        {
            connection.Open();
            Console.WriteLine("Deleting existing Db");
            try
            {
                string deleteDatabase = @"DROP DATABASE IF EXISTS exceltodb;";
                connection.Execute(deleteDatabase);
                Console.WriteLine("Db deleted");
                connection.Close();
            }
            catch
            {
                Console.WriteLine("No db to delete");
            }
        }

    }

    public void CreateDBTable(List<string> columnNames)
    {
        Console.WriteLine("Creating new Db and Table");
        using (var connection = new SqlConnection(_connectionStringToDbUpdate))
        {
            connection.Open();
            string createDatabase = @"CREATE DATABASE exceltodb";
            connection.Execute(createDatabase);
            connection.Close();
        }
        using (var connection = new SqlConnection(_connectionString))
        {
            string createTable = "CREATE TABLE exceldata ([Id] INT IDENTITY(1,1) PRIMARY KEY (Id))";
            connection.Execute(createTable);

            foreach (var column in columnNames)
            {
                string addColumn = $"ALTER TABLE exceldata ADD {column} TEXT";
                connection.Execute(addColumn);
            }
        }
    }

    public void InsertRowsToDb(List<dynamic> rows)
    {
        Console.WriteLine("Inserting data to Db");
        var InsertData = MapRowsToQueryValues(rows);
        using (var connection = new SqlConnection(_connectionString))
        {
            foreach (var column in InsertData.Item2)
            {
                string insertQuery = $"INSERT INTO exceldata ({InsertData.Item1}) VALUES ({column})";
                connection.Execute(insertQuery);
            }
        }
    }

    private (string, List<string>) MapRowsToQueryValues(List<dynamic> rows)
    {
        List<string> queryValues = new();
        string columnNamesForQuery = string.Join(",", ExcelFileHandler._colNames).TrimEnd(',');

        foreach (var row in rows)
        {
            string values = "";
            var propertyBag = (IDictionary<string, object>)row;
            foreach (var property in propertyBag)
            {
                values += "'" + property.Value + "'" + ",";

            }
            values = values.TrimEnd(',');
            queryValues.Add(values);
        }
        return (columnNamesForQuery, queryValues);
    }

    public IEnumerable<dynamic> GetDbData()
    {
        IEnumerable<dynamic> data = new List<dynamic>();
        using var connection = new SqlConnection(_connectionString);
        string query = "SELECT * FROM exceldata";
        var command = new SqlCommand(query, connection);
        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            IDictionary<string, object> row = new Dictionary<string, object>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string columnName = reader.GetName(i);
                object value = reader.GetValue(i);

                row[columnName] = value;
            }

            yield return CreateDynamicObject(row);
        }
        connection.Close();
    }

    public static dynamic CreateDynamicObject(IDictionary<string, object> properties)
    {
        ExpandoObject expandoObject = new ExpandoObject();
        var expandoDict = (IDictionary<string, object>)expandoObject;

        foreach (var property in properties)
        {
            expandoDict[property.Key] = property.Value;
        }

        return expandoObject;
    }
}


