using ExcelReader.Services;

namespace ExcelReader.Controllers;

public class ExcelWorkSheetController(ExcelWorkSheetService excelService)
{
    private readonly ExcelWorkSheetService ExcelServiceInstance = excelService;

    public void TryConnection()
    {
        try
        {
            ExcelServiceInstance.TryConnection();
        }
        catch
        {
            throw;
        }
    }
}