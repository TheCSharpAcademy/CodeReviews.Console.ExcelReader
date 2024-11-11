using Microsoft.Data.Sqlite;

namespace ExcelReader.hasona23;

public class Database
{
    private readonly string _connectionString;
    private readonly string _tableName;
    private readonly ExcelFileHandler _excelFileHandler;

    public Database(ExcelFileHandler excelFileHandler)
    {
        _connectionString = $"Data Source = {excelFileHandler.Worksheet.Name}.db;";
        _tableName = $"BikeStore";
        CreateTable();
        _excelFileHandler = excelFileHandler;
        List<string> colNames = [];
        for (int col = 1; col <= excelFileHandler.ColumnCount; col++)
        {
            colNames.Add(excelFileHandler.Worksheet.Cells[1, col].Text);
        }

        Console.WriteLine($"Table {_tableName} has been created");
        AddTableColumns(colNames);
    }

    private void AddTableColumns(List<string> tableColumns)
    {
        using (var conn = new SqliteConnection(_connectionString))
        {
            Console.WriteLine("Adding Columns...");
            conn.Open();
            using (var command = conn.CreateCommand())
            {
                try
                {
                    foreach (var column in tableColumns)
                    {
                        string addColumn = $"ALTER TABLE {_tableName} ADD {column} TEXT";
                        command.CommandText = addColumn;
                        command.ExecuteNonQuery();
                        Console.WriteLine($"Added {column}...");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            conn.Close();
        }
    }

    private void CreateTable()
    {
        string deletionCommand = $"DROP TABLE IF EXISTS  {_tableName};";
        string creationCommand = $@"CREATE TABLE {_tableName} ([ID] INTEGER  PRIMARY KEY AUTOINCREMENT )";
        using (var conn = new SqliteConnection(_connectionString))
        {
            conn.Open();
            Console.WriteLine("Cleaning Database...");
            using (var command = new SqliteCommand(deletionCommand, conn))
            {
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Creating Database...");
            using (var command = new SqliteCommand(creationCommand, conn))
            {
                command.ExecuteNonQuery();
            }

            conn.Close();
        }
    }

    public void TransferData()
    {
        Console.WriteLine("Transferring data...");
        List<string> rowData = [];
        using (var conn = new SqliteConnection(_connectionString))
        {
            conn.Open();
            try
            {
                using var command = conn.CreateCommand();
                for (int row = 2; row <= _excelFileHandler.RowCount; row++)
                {
                    for (int col = 1; col <= _excelFileHandler.ColumnCount; col++)
                    {


                        rowData.Add(_excelFileHandler.Worksheet.Cells[row, col].Text.Trim());

                    }

                    string cmdText =
                        $@"INSERT INTO {_tableName} VALUES(NULL,'{rowData[0]}','{rowData[1]}','{rowData[2]}','{rowData[3]}','{rowData[4]}','{rowData[5]}','{rowData[6]}')";
                    command.CommandText = cmdText;
                    command.ExecuteNonQuery();
                    rowData.Clear();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            conn.Close();
        }
    }

    public List<List<string>> GetData()
    {
        Console.WriteLine("Getting Data from Database");
        List<List<string>> data = [];
        string getCommand = $"SELECT bike_id,brand,model,price,color,size,weight FROM  {_tableName}";
        using (var conn = new SqliteConnection(_connectionString))
        {
            conn.Open();
            try
            {
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = getCommand;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        data.Add([
                            reader.GetString(0), reader.GetString(1),
                            reader.GetString(2), reader.GetString(3),
                            reader.GetString(4), reader.GetString(5),
                            reader.GetString(6)
                        ]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            conn.Close();
        }

        return data;
    } 
}