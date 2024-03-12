using Newtonsoft.Json;
using OfficeOpenXml;
using System.Data;
using System.Data.SQLite;

namespace ExcelReader.StanimalTheMan;

class Program
{
    static string projectRootFolder = Directory.GetCurrentDirectory();
    static string dbFileName = "fantasy_players.db";
    static string dbFilePath = Path.Combine(projectRootFolder, dbFileName);
    static void Main(string[] args)
    {
        CreateDatabase();

        var baseballPlayers = ReadExcelDriver();

        AddToDB(baseballPlayers);

        ShowTable();
    }

    private static void ShowTable()
    {
        using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFilePath}"))
        {
            connection.Open();

            string fetchDataQuery = @"
                SELECT * FROM BaseballPlayers;
            );";

            Console.WriteLine("Showing data in console after fetching from db");
            using (SQLiteCommand command = new SQLiteCommand(fetchDataQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("ID\tFirstName\tLastName\tPosition\tTeam");
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string firstName = reader.GetString(1);
                        string lastName = reader.GetString(2);
                        string position = reader.GetString(3);
                        string team = reader.GetString(4);
                        if (id == 1)
                        {
                            // tried to use console table ext nuget package but was lazy so I am manually handling Fernando Tatis Jr., a long name and the formatting of the rest of row's data
                            Console.WriteLine($"{id}\t{firstName}\t{lastName}\t{position}\t\t{team}");
                        } else Console.WriteLine($"{id}\t{firstName}\t\t{lastName}\t\t{position}\t\t{team}");
                    }
                }
            }
        }
    }

    private static void AddToDB(List<BaseballPlayer> baseballPlayers)
    {
        Console.WriteLine("Adding to db");
        string insertQuery = @"
            INSERT INTO BaseballPlayers (FirstName, LastName, Position, Team) VALUES (@FirstName, @LastName, @Position, @Team);
        ";

        using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFilePath}"))
        {
            connection.Open();
            foreach (BaseballPlayer player in baseballPlayers)
            {
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", player.FirstName);
                    command.Parameters.AddWithValue("@LastName", player.LastName);
                    command.Parameters.AddWithValue("@Position", player.Position);
                    command.Parameters.AddWithValue("@Team", player.Team);

                    command.ExecuteNonQuery();
                }
            }
        }
    }

    private static void CreateDatabase()
    {

        Console.WriteLine(dbFilePath);
        // check if sqlite db exists and delete it
        if (File.Exists(dbFilePath))
        {
            File.Delete(dbFilePath);
            Console.WriteLine("database successfully deleted");
        }
        else
        {
            Console.WriteLine("database doesn't exist");
        }

        // recreate database file
        SQLiteConnection.CreateFile(dbFilePath);

        try
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFilePath}"))
            {
                connection.Open();
                string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS BaseballPlayers (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FirstName TEXT,
                LastName TEXT,
                Position TEXT,
                Team TEXT
            );";

                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Tables created successfully.");

                
            }
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"SQLite Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General Error: {ex.Message}");
        }
    }

    private static List<BaseballPlayer> ReadExcelDriver()
    {
        Console.WriteLine("Reading from Excel");

        string excelFile = "MyFantasySquad.xlsx";
        List<BaseballPlayer> baseballPlayers = ReadFromExcel<List<BaseballPlayer>>(Path.Combine(projectRootFolder, excelFile));

        Console.WriteLine($"Read {baseballPlayers.Count} records from Excel");

        return baseballPlayers;
    }

    private static T ReadFromExcel<T>(string path, bool hasHeader = true)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var excelPack = new ExcelPackage())
        {
            using (var stream = File.OpenRead(path))
            {
                excelPack.Load(stream);
            }

            var ws = excelPack.Workbook.Worksheets[0];

            DataTable excelasTable = new DataTable();
            foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
            {
                if (!string.IsNullOrEmpty(firstRowCell.Text))
                {
                    string firstColumn = string.Format("Column {0}", firstRowCell.Start.Column);
                    excelasTable.Columns.Add(hasHeader ? firstRowCell.Text : firstColumn);
                }
            }
            var startRow = hasHeader ? 2 : 1;

            for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                var wsRow = ws.Cells[rowNum, 1, rowNum, excelasTable.Columns.Count];
                DataRow row = excelasTable.Rows.Add();
                foreach (var cell in wsRow)
                {
                    row[cell.Start.Column - 1] = cell.Text;
                }
            }

            var generatedType = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(excelasTable));
            return (T)Convert.ChangeType(generatedType, typeof(T));
        }
    }
}
