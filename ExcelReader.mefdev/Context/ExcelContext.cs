using ExcelReader.mefdev.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.mefdev.Context;

public class ExcelContext : DbContext
{
    public DbSet<FinancialData> FinancialData { get; set; }


    public ExcelContext(DbContextOptions<ExcelContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FinancialData>().ToTable("FinancialData");
    }
}
