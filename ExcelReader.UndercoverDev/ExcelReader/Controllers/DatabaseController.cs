using ExcelReader.Models;
using ExcelReader.Services;
using ExcelReader.Utilities;
using Spectre.Console;

namespace ExcelReader.Controllers;
public class DatabaseController
{
    private readonly DatabaseService _databaseService;
    private readonly ExcelReaderController _excelReader;

    public DatabaseController(DatabaseService databaseService, ExcelReaderController excelReader, Logger logger)
    {
        _databaseService = databaseService;
        _excelReader = excelReader;
    }

    public void InitializeDatabase()
    {
        Logger.Log("Deleting existing database...");
        _databaseService.DeleteDatabase();

        Logger.Log("Creating new database...");
        _databaseService.CreateDatabase();

        Logger.Log("Reading data from Excel...");
        var data = _excelReader.ReadExcelData();

        Logger.Log("Inserting data into database...");
        _databaseService.InsertData(data);
    }

    public List<DataModel> FetchData()
    {
        return _databaseService.GetAllData();
    }

    public void DisplayData()
    {
        var data = FetchData();

        Logger.Log("Displaying data:");
        Logger.DisplayData(data);
    }
}