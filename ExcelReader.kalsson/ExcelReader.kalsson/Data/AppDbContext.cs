using ExcelReader.kalsson.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.kalsson.Data;

public class AppDbContext : DbContext
{
    public DbSet<EmployeeModel> Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=employees.db");
    }
}