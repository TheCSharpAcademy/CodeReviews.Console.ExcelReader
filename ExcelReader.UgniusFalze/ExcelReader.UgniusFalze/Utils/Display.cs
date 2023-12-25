using System.Globalization;
using ExcelReader.UgniusFalze.Models;
using Spectre.Console;

namespace ExcelReader.UgniusFalze.Utils;

public static class Display
{
    public static void DisplayStart()
    {
        Console.WriteLine("Program started");
    }
    
    public static void DisplayDatabaseDrop()
    {
        Console.WriteLine("Dropping database");
    }
    
    public static void DisplayDatabaseCreate()
    {
        Console.WriteLine("Creating new database");
    }
    public static void DisplayTableCreate()
    {
        Console.WriteLine("Creating new table for excel data");
    }
    public static void DisplayReadingFile()
    {
        Console.WriteLine("Reading data from file");
    }
    public static void DisplayWritingToDatabase()
    {
        Console.WriteLine("Writing excel data to database");
    }

    public static void DisplayDone()
    {
        Console.WriteLine("Done");
    }

    private static void DisplayWhatWasRead()
    {
        Console.WriteLine("Data that was written to database: ");
    }

    public static void DisplayIncorrectExcel()
    {
        Console.WriteLine("Failed to read or parse data from excel");
    }

    public static void DisplayNotFound()
    {
        Console.WriteLine("No data found in specified file or file not found");
    }

    public static void DisplayData(List<Plane> planes, List<string> columns)
    {
        DisplayWhatWasRead();
        var table = new Table();
        table.AddColumns(columns.ToArray());
        foreach (var planeFromDb in planes)
        {
            table.AddRow(planeFromDb.Manufacturer, planeFromDb.Model, planeFromDb.Type, 
                planeFromDb.MaxSpeed.ToString(), planeFromDb.Capacity.ToString(), planeFromDb.FirstFlightDate.ToString(CultureInfo.CurrentCulture));
        }
        AnsiConsole.Write(table);
    }
}