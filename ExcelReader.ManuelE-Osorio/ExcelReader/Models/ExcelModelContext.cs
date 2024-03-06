using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Models;

public class ExcelModelContext(DbContextOptions<ExcelModelContext> options) : DbContext(options)
{
    public DbSet<ExcelWorkSheetModel> ExcelWorkSheet {get; set;}
    public DbSet<ExcelRowModel> ExcelRow {get; set;}
    public DbSet<ExcelRowStringModel> StringRow {get; set;}
    public DbSet<ExcelRowDataModel<int>> IntRow {get; set;}
    public DbSet<ExcelRowDataModel<DateTime>> DateRow {get; set;}
    public DbSet<ExcelRowDataModel<double>> DoubleRow {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExcelWorkSheetModel>(workSheet =>
        {
            workSheet.HasKey( p => p.WorkSheetId);      
            workSheet.OwnsMany(p => p.Rows, rows =>
            {
                rows.HasKey( p => p.RowId);
                rows.OwnsMany(p => p.StringRows);
                rows.OwnsMany( p => p.IntRows);
                rows.OwnsMany( p => p.DoubleRows);
                rows.OwnsMany( p => p.DateRows);
            });
        });
   }
}