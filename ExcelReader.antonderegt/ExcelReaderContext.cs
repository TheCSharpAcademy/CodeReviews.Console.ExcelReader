using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExcelReader;

public class ExcelReaderContext : DbContext
{
    public DbSet<Number> Numbers { get; set; }
    private string connectionString { get; set; }

    public ExcelReaderContext(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }
}