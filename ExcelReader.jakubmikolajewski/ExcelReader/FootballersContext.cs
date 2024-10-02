using Microsoft.EntityFrameworkCore;
using ExcelReader.Models;

namespace ExcelReader;
internal class FootballersContext : DbContext
{
    public FootballersContext()
    {    
    }
    public FootballersContext(DbContextOptions<FootballersContext> options) : base(options)
    {    
    }

    DbSet<Footballer> Footballers { get; set; }
}
