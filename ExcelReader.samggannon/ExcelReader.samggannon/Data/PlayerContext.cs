using ExcelReader.samggannon.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ExcelReader.samggannon.Data;

internal class PlayerContext : DbContext
{
    public DbSet<Player> PlayerData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString);
    }
}
  