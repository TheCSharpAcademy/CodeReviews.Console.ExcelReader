using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Models;

public class ExcelModelContext(DbContextOptions<ExcelModelContext> options) : DbContext(options)
{
    public DbSet<ExcelWorkSheetModel> ExcelWorkSheet {get; set;}
    public DbSet<ExcelRowModel> ExcelRow {get; set;}
    public DbSet<ExcelCellString> StringCells {get; set;}
    public DbSet<ExcelCellData<int>> IntCells {get; set;}
    public DbSet<ExcelCellData<DateTime>> DateCells {get; set;}
    public DbSet<ExcelCellData<double>> DoubleCells {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExcelWorkSheetModel>(workSheet =>
        {
            workSheet.HasKey( p => p.WorkSheetId);      
            workSheet.OwnsMany(p => p.Rows, rows =>
            {
                rows.HasKey( p => p.RowId);
                rows.Property( p => p.RowId)
                    .ValueGeneratedNever();
                rows.OwnsMany(p => p.StringCells);
                rows.OwnsMany( p => p.IntCells);
                rows.OwnsMany( p => p.DoubleCells);
                rows.OwnsMany( p => p.DateCells);
            });
        });
   }
}