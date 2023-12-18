using ExcelReader.Speedierone.Model;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Speedierone
{
    public class OrdersContext : DbContext
    {
        public DbSet<Orders> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");
            optionsBuilder.UseSqlServer(configuration);
        }
    }
}
