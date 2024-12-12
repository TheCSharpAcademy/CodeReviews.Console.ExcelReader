
using ExcelReader.mefdev.Context;
using ExcelReader.mefdev.Controllers;
using ExcelReader.mefdev.Models;
using ExcelReader.mefdev.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<ExcelContext>();
                var excelReader = scope.ServiceProvider.GetRequiredService<ExcelManager>();

                var data = excelReader.GetFinancialData(@$"{GetCurrentPath()}./Data/dataset.xlsx");

                SaveToDb(context, data);

                var financialData = GetFinancialData(context);

                var excelController = new ExcelController();

                excelController.DisplayFinancialData(financialData);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
    static List<FinancialData> GetFinancialData(ExcelContext context)
    {
        return context.FinancialData.ToList();
    }
    static void SaveToDb(ExcelContext context, List<FinancialData> data)
    {
        context.FinancialData.AddRange(data);
        context.SaveChanges();
    }


    static string GetCurrentPath()
    {
        return Environment.CurrentDirectory.Replace("bin/Debug/net8.0", "");
    }

    static string? GetConnectionString()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(GetCurrentPath())
            .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

        var _configuration = builder.Build();
        return _configuration.GetConnectionString("DefaultConnection");
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) =>
           {
               services.AddDbContext<ExcelContext>(options =>
                   options.UseSqlServer(
                       GetConnectionString(), options => options.EnableRetryOnFailure()
                   )
               );
               services.AddScoped<ExcelManager>();
           });
}