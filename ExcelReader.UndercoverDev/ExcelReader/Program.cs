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
    controller.RunApp();
}
