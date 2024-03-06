using ExcelReader.Controllers;
using ExcelReader.Models;
using ExcelReader.Repositories;
using ExcelReader.Services;
using ExerciseTracker.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExcelReader;

public class StartUp
{
    public static IHost AppInit()
    {
        var appBuilder = new HostBuilder();
        appBuilder.ConfigureAppConfiguration(p =>
            p.AddJsonFile("appsettings.json").Build());

        appBuilder.ConfigureServices((host, services) =>
        {
            services.AddDbContext<ExcelModelContext>(options => options
                .UseSqlServer(host.Configuration.GetConnectionString("DefaultConnection"))
                .LogTo(Console.WriteLine));

            AppVars appVars = host.Configuration.GetSection("Settings").Get<AppVars>() ?? new AppVars();
            services.AddSingleton(appVars);
            services.AddScoped<IExcelRepository<ExcelWorkSheetModel>, ExcelWorkSheetRepository>();
            services.AddScoped<ExcelWorkSheetService>();
            services.AddScoped<ExcelDataService>();
            services.AddScoped<ExcelWorkSheetController>();
            services.AddScoped<DataController>();
        });

        var app = appBuilder.Build();
        var excelController = app.Services.CreateScope()
            .ServiceProvider.GetRequiredService<ExcelWorkSheetController>();
        excelController.TryConnection();

        return app;
    }
}