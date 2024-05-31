using ExcelReader;
using ExcelReader.Data;
using ExcelReader.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main()
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Set up the service collection
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection, configuration);

        // Build the service provider
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Run the display program with the service provider
        var display = serviceProvider.GetService<Display>();
        await display.RunProgramAsync();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext with connection string from configuration
        services.AddDbContext<ExcelContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Add the service
        services.AddScoped<CheckService>();

        // Add the display class
        services.AddScoped<Display>();
    }
}