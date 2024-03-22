using ExcelReader.frockett.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.frockett.Database;

internal class EmployeeContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Employees.db");
    }
}
