using ExcelReader.Models;
using ExcelReader.Services;
using ExcelReader.UI;
using OfficeOpenXml.Drawing.Chart;

namespace ExcelReader.Controllers;

public class DataController(ExcelWorkSheetController excelWorkSheetController, ExcelDataService excelDataService)
{
    private readonly ExcelWorkSheetController ExcelControllerInstance = excelWorkSheetController;
    private readonly ExcelDataService ExcelDataServiceInstance = excelDataService;
    
    public void Start()
    {
        MainUI.WelcomeMessage();
        Main();
        MainUI.ExitMessage();
    }
    public void Main()
    {
        if(!CreateDatabase())
            return;

        var workSheetData = GetWorkSheetModel();
        if(workSheetData is null)
            return;

        ExcelControllerInstance.Insert(workSheetData);
        var data = ExcelControllerInstance.GetAll();
        if(data is null || !data.Any())
            return;

        MainUI.DisplayData(data.ToList().First());
        MainUI.InformationMessage("Press any key to continue.");
        Console.ReadKey();
        return;
    }

    public bool CreateDatabase()
    {
        MainUI.InformationMessage("Creating the database.");
        try
        {
            ExcelControllerInstance.TryConnection();
        }
        catch (Exception e)
        {
            MainUI.ErrorMessage(e.Message);
            return false;
        }
        MainUI.InformationMessage("Database creation succesful!");
        return true;
    }

    public ExcelWorkSheetModel? GetWorkSheetModel()
    {
        try
        {
            return ExcelDataServiceInstance.GetWorkSheetModel();
        }
        catch (Exception e)
        {
            MainUI.ErrorMessage(e.Message);
            return null;
        }
    }
}