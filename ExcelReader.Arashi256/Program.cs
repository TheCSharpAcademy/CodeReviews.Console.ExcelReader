using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using ExcelReader.Arashi256.Config;
using ExcelReader.Arashi256.Controllers;
using ExcelReader.Arashi256.Models;
using ExcelReader.Arashi256.Services;
using ExcelReader.Arashi256.Interfaces;

try
{
    IHostBuilder builder = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        })
        .ConfigureServices((context, services) =>
        {
            // Register classes.
            services.AddDbContext<ExcelReaderContext>();
            services.AddSingleton<AppManager>();
            services.AddScoped<IFileInputService, ExcelReaderService>();    // Implements interface so can use alternative external input file. Change appsettings.json
            //services.AddScoped<IFileService, CsvReaderService>();         // Implements interface so can use alternative external input file. Change appsettings.json
            services.AddScoped<IDatabaseService, DatabaseService>();        // Implements interface so can use alternative db engine/ORM. 
            services.AddScoped<IFileOutputService, ExcelWriterService>();   // Implements interface so can use alternative external output file. Only Excel implemented.
            // Register the controller as hosted service.
            services.AddHostedService<ExcelReaderController>();
        })
        .UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.WriteTo.Console().WriteTo.File("excel-import-.log", rollingInterval: RollingInterval.Day);
        });
    IHost host = builder.Build();
    // Run the hosted service.
    await host.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: '{ex.Message}'");
}