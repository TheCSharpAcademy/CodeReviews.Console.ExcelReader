using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ExcelReader.Controllers;
using ExcelReader.Services;
using ExcelReader.Utilities;
using ExcelReader.Data;
using Microsoft.Extensions.Configuration;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddSingleton<DatabaseController>();
builder.Services.AddSingleton<ExcelReaderController>();
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddSingleton<ExcelService>();
builder.Services.AddSingleton<Logger>();
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

var scope = app.Services.CreateScope();
var servicesExtension = scope.ServiceProvider;

var controller = servicesExtension.GetRequiredService<DatabaseController>();
controller.InitializeDatabase();
controller.DisplayData();