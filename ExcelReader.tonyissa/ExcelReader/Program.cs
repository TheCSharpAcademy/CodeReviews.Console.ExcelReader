using ExcelReader.Data;
using ExcelReader.Models;
using ExcelReader.Repositories;
using ExcelReader.Services;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Spectre.Console;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ExcelContext>(db =>
{
    var dbPath = builder.Configuration.GetConnectionString("DefaultConnection");
    db.UseSqlServer(dbPath);
});

builder.Services.AddScoped(typeof(IExcelRepository<,>), typeof(ExcelRepository<,>));
builder.Services.AddScoped<ExcelReaderService>();
builder.Services.AddScoped<FileProcesserService>();

var host = builder.Build();

using var scope = host.Services.CreateScope();
var scopedProvider = scope.ServiceProvider;

var logger = scopedProvider.GetRequiredService<ILogger<Program>>();
var context = scopedProvider.GetRequiredService<ExcelContext>();

logger.LogInformation("Deleting database...");
context.Database.EnsureDeleted();
logger.LogInformation("Creating database...");
context.Database.EnsureCreated();

var app = scopedProvider.GetRequiredService<ExcelReaderService>();

await app.ExecuteServiceAsync();