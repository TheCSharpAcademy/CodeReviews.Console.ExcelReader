using ExcelReader.Models;
using ExcelReader.Services;
using ExcelReader.Utilities;

namespace ExcelReader.Controllers;
public class DatabaseController
{
    private readonly DatabaseService _databaseService;
    private readonly ExcelReaderController _excelReader;

    public DatabaseController(DatabaseService databaseService, ExcelReaderController excelReader)
    {
        _databaseService = databaseService;
        _excelReader = excelReader;
    }

    public void RunApp()
    {
        InitializeDatabase();
    }

    public void InitializeDatabase()
    {
        Logger.Log("[bold][green]Deleting existing database...[/][/]");
        _databaseService.DeleteDatabase();

        Logger.Log("[bold][green]Creating new database...[/][/]");
        _databaseService.CreateDatabase();

        ReadExcelData();
    }

    public void ReadExcelData()
    {
        Logger.Log("[bold][green]Reading data from Excel...[/][/]");
        var data = _excelReader.ReadExcelData();

        if (data.Count == 0)
        {
            Logger.Log("[bold][yellow]No data found in the Excel file.[/][/]");
            return;
        }
        
        InsertDataIntoDatabase(data);
    }

    private void InsertDataIntoDatabase(List<DataModel> data)
    {
        Logger.Log("[bold][green]Inserting data into database...[/][/]");
        _databaseService.InsertData(data);
        Logger.Log("[bold][green]Data inserted into database.[/][/]");

        DisplayData();
    }

    public List<DataModel> FetchData()
    {
        return _databaseService.GetAllData();
    }

    public void DisplayData()
    {
        var data = FetchData();
        
        if (data.Count == 0)
        {
            Logger.Log("[bold][yellow]No data found in the database.[/][/]");
            return;
        }

        Logger.Log("[bold][green]Displaying data:[/][/]");
        Logger.DisplayData(data);
    }
}