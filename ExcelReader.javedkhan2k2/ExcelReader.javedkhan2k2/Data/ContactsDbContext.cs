using ExcelReader.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExcelReader.Data;

public class ContactsDbContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder().AddUserSecrets<ContactsDbContext>();
        var configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration["ConnectionString.DefaultConnection"]);
    }

}