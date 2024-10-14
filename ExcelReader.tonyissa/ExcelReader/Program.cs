using ExcelReader.Data;
using ExcelReader.Repositories;
using ExcelReader.Services;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

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

var app = scopedProvider.GetRequiredService<ExcelReaderService>();

await app.ExecuteServiceAsync();