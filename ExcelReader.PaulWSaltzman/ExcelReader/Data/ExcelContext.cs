using ExcelReader.Models;
using Microsoft.EntityFrameworkCore;


namespace ExcelReader.Data
{
    public class ExcelContext : DbContext
    {
        public ExcelContext(DbContextOptions<ExcelContext> options)
            : base(options)
        {
        }
        public DbSet<Check> Checks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Check>()
                .HasKey(c => c.CheckKid);
            modelBuilder.Entity<Check>()
                .Property(c => c.Amount)
                .HasColumnType("decimal(18, 2)");
        }

    }
}




