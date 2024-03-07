using ExcelReader.Models;
using ExcelReader.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Repositories;

public class ExcelWorkSheetRepository(ExcelModelContext dbContext): IExcelRepository<ExcelWorkSheetModel>
{
    private readonly ExcelModelContext  DbContext = dbContext;

    public bool TryConnection()
    {
        try
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();
            DbContext.Database.OpenConnection();
            DbContext.Database.CanConnect();
            return true;
        }
        catch 
        {
            throw new Exception("The app cannot connect to the Database. "+
                "Please check your Connection String configuration in your appsettings.json");
        }
    }

    public bool Insert(ExcelWorkSheetModel model)
    {
        DbContext.ExcelWorkSheet.Add(model);
        DbContext.SaveChanges();
        return true;
    }

    public IEnumerable<ExcelWorkSheetModel> GetAll()
    {
        return DbContext.ExcelWorkSheet.AsEnumerable();
    }
    
    public ExcelWorkSheetModel? GetById(int id)
    {
        return DbContext.ExcelWorkSheet.Find(id);
    }

    public bool Update(ExcelWorkSheetModel model)
    {
        var workSheetToUpdate = GetById(model.WorkSheetId);
        if ( workSheetToUpdate == null)
            return false;

        DbContext.Entry(workSheetToUpdate).CurrentValues.SetValues(model);
        DbContext.SaveChanges();
        return true;
    }

    public bool Delete(ExcelWorkSheetModel model)
    {
        var workSheetToDelete = DbContext.ExcelWorkSheet.Where(
            p => p.WorkSheetId == model.WorkSheetId).First();

        if ( workSheetToDelete == null)
            return false;

        DbContext.Remove(workSheetToDelete);
        DbContext.SaveChanges();
        return true;
    }
}