using ExcelReader.StevieTV.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.StevieTV.Database;

public class EmployeeContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("server=localhost;initial catalog=employees;Trusted_Connection=True;Integrated Security=SSPI;TrustServerCertificate=True");
    }
}