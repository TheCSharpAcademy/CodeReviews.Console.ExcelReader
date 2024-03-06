using ExcelReader.Services;

namespace ExcelReader.Controllers;

public class DataController(ExcelWorkSheetController excelWorkSheetController, ExcelDataService excelDataService)
{
    private ExcelWorkSheetController ExcelControllerInstance = excelWorkSheetController;
    private ExcelDataService ExcelDataServiceInstance = excelDataService;
    
    public void Start()
    {
        var hola = ExcelDataServiceInstance.GetWorkSheetModel();
        Console.WriteLine(true + "Hola");
        return;
    }
}