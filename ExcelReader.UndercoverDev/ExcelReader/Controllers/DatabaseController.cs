using ExcelReader.Models;
using ExcelReader.Services;
using ExcelReader.Utilities;

namespace ExcelReader.Controllers;
public class DatabaseController
{
    private readonly DatabaseService _databaseService;
    private readonly ExcelReaderController _excelReader;
    private readonly Logger _logger;

    public DatabaseController(DatabaseService databaseService, ExcelReaderController excelReader, Logger logger)
    {
        _databaseService = databaseService;
        _excelReader = excelReader;
        _logger = logger;
    }

    public void InitializeDatabase()
    {
        _logger.Log("Deleting existing database...");
        _databaseService.DeleteDatabase();

        _logger.Log("Creating new database...");
        _databaseService.CreateDatabase();

        _logger.Log("Reading data from Excel...");
        var data = _excelReader.ReadExcelData();

        _logger.Log("Inserting data into database...");
        _databaseService.InsertData(data);
    }

    public List<DataModel> FetchData()
    {
        return _databaseService.GetAllData();
    }

    public void DisplayData(List<DataModel> data)
    {
        foreach (var item in data)
        {
            Console.WriteLine($"Date: {item.Date}, League: {item.League}, Home: {item.Home}, Away: {item.Away}, Home Probability: {item.HomeProbability}, Away Probability: {item.AwayProbability}, Over Two Goals: {item.OverTwoGoals}");
        }
    }
}