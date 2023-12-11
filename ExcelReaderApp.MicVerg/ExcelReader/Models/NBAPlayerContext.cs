using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Models
{
    internal class NBAPlayerContext : DbContext
    {
        public DbSet<NBAPlayer> NBAPlayers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-ETA4JL7;Database=ExcelReader;Trusted_Connection=True;Integrated Security=True;Encrypt=False;");
        }
    }
}
