using ExcelReader.ConsoleTableViewer;
using ExcelReader.Controller;
using ExcelReader.Data;
using ExcelReader.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfigurationBuilder builder = new ConfigurationBuilder();

var currentDirectory = Directory.GetCurrentDirectory();
builder.SetBasePath(currentDirectory);
builder.AddJsonFile("appsettings.json");

IConfiguration config = builder.Build();

var services = new ServiceCollection();
services.AddSingleton(config);
services.AddTransient<IDataAccess, DapperDataAccess>();
services.AddTransient<IReaderService, EPPlusReaderService>();
services.AddTransient<ITableVisualization, ConsoleTableBuilderVisualization>();
services.AddTransient<IReaderAppController, ExcelReaderAppController>();
services.AddTransient<ISetupDatabase, SetupDatabase>();

var serviceProvider = services.BuildServiceProvider();
var startup = serviceProvider.GetService<IReaderAppController>();

await startup!.RunProgram();