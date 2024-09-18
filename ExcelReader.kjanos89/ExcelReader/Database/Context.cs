using ExcelReader.Model;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Database
{
    public class Context : DbContext
    {
        public Context() 
        {
            
        }

        public Context(DbContextOptions<Context> options) : base(options) 
        {
            
        }

        public DbSet<ExcelData> ExcelDbSet { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine("Configuring database...");
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ExcelReaderDB"]?.ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
