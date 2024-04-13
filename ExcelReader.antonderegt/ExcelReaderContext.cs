using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExcelReader;

public class ExcelReaderContext : DbContext
{
    public DbSet<Number> Numbers { get; set; }
    private readonly string _connectionString;

    public ExcelReaderContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
}
