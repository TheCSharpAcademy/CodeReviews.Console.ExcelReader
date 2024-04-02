using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExcelReader;

public class ExcelReaderContext : DbContext
{
    public DbSet<Number> Numbers { get; set; }
    public string ConnectionString { get; set; }

    public ExcelReaderContext()
    {
        IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        ConnectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }
}