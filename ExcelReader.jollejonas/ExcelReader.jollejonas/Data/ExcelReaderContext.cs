using ExcelReader.jollejonas.Models;
using Microsoft.EntityFrameworkCore;


namespace ExcelReader.jollejonas.Data;
public class ExcelReaderContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = ExcelReaderContext; Trusted_Connection = True; MultipleActiveResultSets = true");
    }
}

