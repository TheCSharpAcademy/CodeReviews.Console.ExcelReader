using ExcelReader.Database;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Services;

public class SetupService
{
    private ExcelReaderDbContext Db;

    public SetupService(ExcelReaderDbContext db)
    {
        Db = db;
    }

    public void Setup()
    {
        Db.ChangeTracker
            .Entries()
            .ToList()
            .ForEach(e => e.State = EntityState.Detached);

        Db.Database.EnsureDeleted();
        Db.Database.EnsureCreated();
        Db.Database.Migrate();
    }
}