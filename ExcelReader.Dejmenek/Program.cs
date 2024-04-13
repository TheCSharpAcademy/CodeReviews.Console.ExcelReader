using ExcelReader.Dejmenek.Data;
using ExcelReader.Dejmenek.Data.Interfaces;
using ExcelReader.Dejmenek.Data.Repositories;
using ExcelReader.Dejmenek.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();

services.AddSingleton(configuration);
services.AddScoped<ISetupDatabase, SetupDatabase>();
services.AddScoped<IItemsRepository, ItemsRepository>();
services.AddScoped<IExcelReaderService, ExcelReaderService>();

var serviceProvider = services.BuildServiceProvider();

var excelReader = serviceProvider.GetRequiredService<IExcelReaderService>();

excelReader.Run();