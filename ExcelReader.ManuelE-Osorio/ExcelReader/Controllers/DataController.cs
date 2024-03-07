using ExcelReader.Services;

namespace ExcelReader.Controllers;

public class DataController(ExcelWorkSheetController excelWorkSheetController, ExcelDataService excelDataService)
{
    private readonly ExcelWorkSheetController ExcelControllerInstance = excelWorkSheetController;
    private readonly ExcelDataService ExcelDataServiceInstance = excelDataService;
    
    public void Start()
    {
        var workSheetData = ExcelDataServiceInstance.GetWorkSheetModel();
        ExcelControllerInstance.Insert(workSheetData);
        return;
    }
}