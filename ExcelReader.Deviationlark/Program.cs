using Spectre;
using Spectre.Console;

namespace ExcelReader;

class Program
{
    private static readonly Database _database = new();
    private static readonly Reader reader = new();
    static void Main(string[] args)
    {
        DeleteDatabase();
        CreateDatabase();
        reader.ExcelReader();
        ShowTable();
    }

    public static void DeleteDatabase()
    {
        Console.WriteLine("Deleting database");
        if (_database.Database.EnsureDeleted())
        {
            Console.WriteLine("Database Deleted");
        }
        else
        {
            Console.WriteLine("No database found.");
        }
    }

    public static void CreateDatabase()
    {
        Console.WriteLine("Creating database");
        if (_database.Database.EnsureCreated())
        {
            Console.WriteLine("Database created.");
        }
        else
        {
            Console.WriteLine("Database already exists.");
        }
    }
    public static void Insert(Gym gymInfo)
    {
        gymInfo.Id = 0;
        _database.gymInfo.Add(gymInfo);
        _database.SaveChanges();
    }

    public static void ShowTable()
    {
        Console.WriteLine("Creating table");
        var gymInfo = _database.gymInfo.ToList();

        var table = new Table();
        table.AddColumn("Monday");
        table.AddColumn("Tuesday");
        table.AddColumn("Wednesday");
        table.AddColumn("Thursday");
        table.AddColumn("Friday");
        table.AddColumn("Saturday");
        table.AddColumn("Sunday");

        foreach (var info in gymInfo)
        {
            if (info.Monday == null) info.Monday = "";
            if (info.Tuesday == null) info.Tuesday = "";
            if (info.Wednesday == null) info.Wednesday = "";
            if (info.Thursday == null) info.Thursday = "";
            if (info.Friday == null) info.Friday = "";
            if (info.Saturday == null) info.Saturday = "";
            if (info.Sunday == null) info.Sunday = "";

            table.AddRow(info.Monday, 
            info.Tuesday, 
            info.Wednesday, 
            info.Thursday, 
            info.Friday, 
            info.Saturday, 
            info.Sunday);
        }
        AnsiConsole.Write(table);
    }
}