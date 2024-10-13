using ExcelReader.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Data;

public class ExcelContext : DbContext
{
    public DbSet<ExcelFile> ExcelFiles { get; set; }
}