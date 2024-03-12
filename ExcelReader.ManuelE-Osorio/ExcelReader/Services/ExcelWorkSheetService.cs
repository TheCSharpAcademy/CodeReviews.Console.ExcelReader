
using ExcelReader.Models;
using ExcelReader.Repositories;

namespace ExcelReader.Services;

public class ExcelWorkSheetService(IExcelRepository<ExcelWorkSheetModel> excelRepository)
{
    private readonly IExcelRepository<ExcelWorkSheetModel> ExcelRepositoryInstance = excelRepository;

    public bool TryConnection()
    {
        try
        {
            return ExcelRepositoryInstance.TryConnection();
        }
        catch
        {
            throw;
        }
    }

    public IEnumerable<ExcelWorkSheetModel> GetAll()
    {
        return ExcelRepositoryInstance.GetAll();
    }

    public ExcelWorkSheetModel? GetById(int id)
    {
        return ExcelRepositoryInstance.GetById(id);
    }

    public bool Insert(ExcelWorkSheetModel model)
    {
        return ExcelRepositoryInstance.Insert(model);
    }

    public bool Update(ExcelWorkSheetModel model)
    {
        return ExcelRepositoryInstance.Update(model);
    }

    public bool Delete(ExcelWorkSheetModel model)
    {
        return ExcelRepositoryInstance.Delete(model);
    }
}