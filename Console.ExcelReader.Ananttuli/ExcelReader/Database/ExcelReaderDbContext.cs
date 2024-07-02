using ExcelReader.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Database;

public class ExcelReaderDbContext : DbContext
{
    public DbSet<Col> Cols { get; set; }
    public DbSet<Row> Rows { get; set; }
    public DbSet<Cell> Cells { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        var dbPath = System.Configuration.ConfigurationManager.AppSettings["DbPath"] ??
            throw new System.Configuration.ConfigurationErrorsException(
                "DbPath configuration must be defined in App.config"
            );

        builder.UseSqlite($"Data Source={dbPath}");
    }
}