using Microsoft.EntityFrameworkCore;

namespace STUDY.ConsoleP.ExcelReaderTryTwo;
internal class ExcelDbContext : DbContext
{
    public DbSet<ExcelDbModel> ExcelDataModels { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\LocalDBDemo;Database=ExcelReaderDb;Trusted_Connection=True;TrustServerCertificate=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}