using ExcelReader.Arashi256.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace ExcelReader.Arashi256.Models
{
    internal class ExcelReaderContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly AppManager _appManager;

        public ExcelReaderContext(IConfiguration configuration, AppManager appManager)
        {
            _configuration = configuration;
            _appManager = appManager;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = _appManager.GetConnectionString();
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is missing.");
            }
            options.UseSqlite(connectionString);
        }

        public DbSet<Movie> Movies { get; set; }
    }
}
