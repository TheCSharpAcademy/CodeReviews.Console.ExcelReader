using ExcelReader.Models;
using ExcelReader.Services;
using ExcelReader.UI;

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
        MainUI.InformationMessage("Querying the database.");
        IEnumerable<ExcelWorkSheetModel>? data;
        try
        {
            data = ExcelServiceInstance.GetAll();
            MainUI.InformationMessage("Data queried succesfully.");
            if(!data.Any())
                MainUI.ErrorMessage("Database does not contain any element.");
            return data;
        }
        catch (Exception e)
        {
            MainUI.ErrorMessage(e.Message);
            return null;
        }
    }

    public bool Insert(ExcelWorkSheetModel worksheet)
    {
        MainUI.InformationMessage("Saving data model to the database.");
        try
        {
            ExcelServiceInstance.Insert(worksheet);
            MainUI.InformationMessage("Data saved succesfully.");
            return true;
        }
        catch (Exception e)
        {
            MainUI.ErrorMessage(e.Message);
            return false;
        }
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