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
            options.UseSqlServer(configuration.GetConnectionString("ExcelConnection"));
        })
        .AddScoped<IExcelRepository, ExcelRepository>()
        .AddScoped<IUserInterface, UserInterface>()
        .AddScoped<IExcelService, ExcelServices>()
        .AddScoped<IExcelController, ExcelController>();
    }
    public static async Task<int> Main(string[] args)
    {
        using (IHost host = CreateHostBuilder(args).Build()) 
        {
            await host.StartAsync();
            var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
            
            IServiceProvider excelServiceProvider = host.Services;
            IExcelController? excelController = excelServiceProvider.GetRequiredService<IExcelController>();
            excelController!.Run();

            lifetime.StopApplication();
            await host.WaitForShutdownAsync();
        }
        return 0;
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .UseConsoleLifetime()
        .ConfigureServices(ConfigureServices);
}
internal class DatabaseStartup : IHostedService
{
    private readonly IServiceProvider serviceProvider;
    public DatabaseStartup(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ExcelContext>();
            await db.Database.EnsureCreatedAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}