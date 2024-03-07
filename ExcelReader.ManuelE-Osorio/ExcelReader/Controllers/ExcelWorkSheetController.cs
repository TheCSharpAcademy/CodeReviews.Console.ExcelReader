using ExcelReader.Models;
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

    public ExcelWorkSheetModel? GetById(int id)
    {
        return ExcelServiceInstance.GetById(id);
    }

    public IEnumerable<ExcelWorkSheetModel>? GetAll()
    {
        return ExcelServiceInstance.GetAll();
    }

    public bool Insert(ExcelWorkSheetModel worksheet)
    {
        return ExcelServiceInstance.Insert(worksheet);
    }

    
    public bool Update(ExcelWorkSheetModel worksheet)
    {
        return ExcelServiceInstance.Insert(worksheet);
    }

    public bool Delete(ExcelWorkSheetModel worksheet)
    {
        return ExcelServiceInstance.Insert(worksheet);
    }
}