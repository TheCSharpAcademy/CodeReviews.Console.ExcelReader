using ExcelReader.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Data;
public class AppDbContext : DbContext
{
    public DbSet<DataModel> DataModels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=ExcelToDb.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DataModel>().HasNoKey();
    }
}