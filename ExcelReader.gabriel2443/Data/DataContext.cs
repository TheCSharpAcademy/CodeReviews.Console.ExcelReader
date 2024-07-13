using ExcelReader.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Data;

public class DataContext : DbContext
{
    public DbSet<EmployeeDetails> EmployeesDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(LocalDB)\ExcelReaderDb;Initial Catalog=ExcelReaderDb;Integrated Security=true;TrustServerCertificate=True");
    }
}