using System.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader;

public class Database : DbContext
{
    public DbSet<Gym> gymInfo { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Gym>(entity =>
        {
            entity.Property(e => e.Monday)
            .IsRequired(false);

            entity.Property(e => e.Tuesday)
            .IsRequired(false);

            entity.Property(e => e.Wednesday)
            .IsRequired(false);

            entity.Property(e => e.Thursday)
            .IsRequired(false);

            entity.Property(e => e.Friday)
            .IsRequired(false);

            entity.Property(e => e.Saturday)
            .IsRequired(false);

            entity.Property(e => e.Sunday)
            .IsRequired(false);
        });
    }
}