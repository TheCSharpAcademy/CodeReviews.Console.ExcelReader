using ExcelReader.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Data;

public class CarContext : DbContext
{
    public CarContext()
    {

    }
    public CarContext(DbContextOptions<CarContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.;Database=CarsDb;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=False");


    }

    public DbSet<Car> Cars { get; set; }
}

