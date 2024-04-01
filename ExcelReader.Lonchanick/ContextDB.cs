using exelReader2._0.Models;
using Microsoft.EntityFrameworkCore;

namespace exelReader2._0;

internal class ContextDB : DbContext
{ 

    public DbSet<Person> Persons { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
        optionsBuilder.UseSqlite($"Data Source =  Persons.db");

    }



}
