using ExcelReader.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Data;

public class ExcelContext(DbContextOptions<ExcelContext> options) : DbContext(options)
{
    public DbSet<ExcelData> Data { get; set; }
}