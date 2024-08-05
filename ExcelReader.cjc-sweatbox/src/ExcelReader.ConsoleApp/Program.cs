using ExcelReader.ConsoleApp.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ExcelReader.ConsoleApp;

/// <summary>
/// Main insertion point for the console application.
/// Configures the application as a HostedServices and launches as a Console.
/// </summary>
internal class Program
{
    #region Methods

    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, false)
            .AddEnvironmentVariables()
            .Build())
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            Log.Information("Application starting");

            var builder = Host.CreateDefaultBuilder(args);

            builder.AddDependancies();

            await builder.RunConsoleAsync();

            Log.Information("Application closing");
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Failed to start application");
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    #endregion
}
