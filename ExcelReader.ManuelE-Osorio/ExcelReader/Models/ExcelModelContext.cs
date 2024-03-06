using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Models;

public class ExcelModelContext() : DbContext
{
    public DbSet<ExcelRowModel> ExcelRow {get; set;}
    public DbSet<ExcelRowStringModel> StringRow {get; set;}
    public DbSet<ExcelRowDataModel<int>> IntRow {get; set;}
    public DbSet<ExcelRowDataModel<DateTime>> DateRow {get; set;}
    public DbSet<ExcelRowDataModel<double>> DoubleRow {get; set;}

    private const string ConnectionString = "Data Source=localhost;Database='ExcelReaderProgram';Integrated Security=false;Encrypt=false;MultipleActiveResultSets=true;User ID=sa;Password=Root1234";

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options
        .UseSqlServer(ConnectionString,
        sqlServerOptions => sqlServerOptions.CommandTimeout(5))
        .LogTo(Console.WriteLine);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExcelRowModel>()
            .HasKey(p => p.RowId);
        
        modelBuilder.Entity<ExcelRowModel>()
            .OwnsMany(p => p.StringRows);
        
        modelBuilder.Entity<ExcelRowModel>()
            .OwnsMany(p => p.IntRows);

        modelBuilder.Entity<ExcelRowModel>()
            .OwnsMany(p => p.DateRows);

        modelBuilder.Entity<ExcelRowModel>()
            .OwnsMany(p => p.DoubleRows);
    }
}