using Microsoft.EntityFrameworkCore;
using STUDY.ConsoleP.ExcelReader.Models;

namespace STUDY.ConsoleP.ExcelReader
{
    internal class ExcelDbContext : DbContext
    {
        public DbSet<ExcelDataModel> ExcelDataModels { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\LocalDBDemo;Initial Catalog=ExcelReaderDB;Integrated Security=True;MultipleActiveResultSets=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the table and columns using hardcoded SQL
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<ExcelDataModel>()
                .ToTable("ExcelDataTable")
                .HasKey(e => e.Id);

            modelBuilder.Entity<ExcelDataModel>()
                .Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<ExcelDataModel>()
                .Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);
        }

    }   
}
