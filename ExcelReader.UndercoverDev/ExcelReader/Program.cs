// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using ExcelReader.Controllers;
// using ExcelReader.Services;
// using ExcelReader.Utilities;
// using ExcelReader.Data;
// using Microsoft.Extensions.Configuration;

// HostApplicationBuilder builder = Host.CreateApplicationBuilder();

// builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// builder.Services.AddSingleton<DatabaseController>();
// builder.Services.AddSingleton<ExcelReaderController>();
// builder.Services.AddSingleton<DatabaseService>();
// builder.Services.AddSingleton<ExcelService>();
// builder.Services.AddSingleton<Logger>();
// builder.Services.AddDbContext<AppDbContext>();

// var app = builder.Build();

// var scope = app.Services.CreateScope();
// var servicesExtension = scope.ServiceProvider;

// var controller = servicesExtension.GetRequiredService<DatabaseController>();
// controller.InitializeDatabase();
// controller.DisplayData();


using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ExcelReader.Controllers;
using ExcelReader.Services;
using ExcelReader.Utilities;
using ExcelReader.Data;
using Microsoft.Extensions.Configuration;

var builder = Host.CreateApplicationBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

InitializeDatabase(app.Services);

// await app.RunAsync();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddSingleton<DatabaseController>();
    services.AddSingleton<ExcelReaderController>();
    services.AddSingleton<DatabaseService>();
    services.AddSingleton<ExcelService>();
    services.AddSingleton<Logger>();
    services.AddDbContext<AppDbContext>();
}

void InitializeDatabase(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var controller = scope.ServiceProvider.GetRequiredService<DatabaseController>();
    controller.InitializeDatabase();
    controller.DisplayData();
}
