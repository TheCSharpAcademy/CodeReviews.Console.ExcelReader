using ExcelReader.Forser.Context;
using ExcelReader.Forser.Controllers;
using ExcelReader.Forser.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    private static readonly IConfiguration configuration;

    static Program()
    {
        configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    }
    public static void ConfigureServices(HostBuilderContext context, IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<ExcelContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ExcelConnection"),
                providerOptions => { providerOptions.EnableRetryOnFailure(); });
        })
        .AddScoped<IExcelRepository, ExcelRepository>()
        .AddScoped<IUserInterface, UserInterface>()
        .AddScoped<IExcelService, ExcelServices>()
        .AddScoped<IExcelController, ExcelController>();
    }
    public static void Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();
                       
        IServiceProvider excelServiceProvider = host.Services;
        IExcelController? excelController = excelServiceProvider.GetRequiredService<IExcelController>();
        excelController!.Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices(ConfigureServices);
}