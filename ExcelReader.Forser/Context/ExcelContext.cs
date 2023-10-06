using Microsoft.EntityFrameworkCore.Storage;

namespace ExcelReader.Forser.Context
{
    public class ExcelContext : DbContext
    {
        public DbSet<HockeyModel> Players { get; set; }
        public ExcelContext(DbContextOptions options) : base(options) { }
    }
}