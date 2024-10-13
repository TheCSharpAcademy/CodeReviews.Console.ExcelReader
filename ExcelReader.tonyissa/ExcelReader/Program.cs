using ExcelReader.Data;
using ExcelReader.Repositories;
using ExcelReader.Services;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ExcelContext>(db =>
{
    var dbPath = builder.Configuration.GetConnectionString("DefaultConnection");
    db.UseSqlServer(dbPath);
});

builder.Services.AddTransient<FileProcesserService>();
builder.Services.AddScoped(typeof(IExcelRepository<>), typeof(ExcelRepository<,>));

var host = builder.Build();