using ExcelReader.Data;
using ExcelReader.Models;

namespace ExcelReader.Services;
public class DatabaseService
{
    private readonly AppDbContext _dbContext;

    public DatabaseService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    internal void DeleteDatabase()
    {
        _dbContext.Database.EnsureDeleted();
    }

    internal void CreateDatabase()
    {
        _dbContext.Database.EnsureCreated();
    }

    internal void InsertData(List<DataModel> data)
    {
        _dbContext.DataModels.AddRange(data);
        _dbContext.SaveChanges();
    }

    internal List<DataModel> GetAllData()
    {
        return [.. _dbContext.DataModels];
    }
}