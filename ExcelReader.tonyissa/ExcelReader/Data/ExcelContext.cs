using ExcelReader.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Data;

public class ExcelContext(DbContextOptions<ExcelContext> options) : DbContext(options)
{
    public DbSet<ExcelData> Data { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExcelData>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<ExcelData>()
            .Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<ExcelData>()
            .Property(e => e.Amount)
            .HasPrecision(18, 2);
    }
}