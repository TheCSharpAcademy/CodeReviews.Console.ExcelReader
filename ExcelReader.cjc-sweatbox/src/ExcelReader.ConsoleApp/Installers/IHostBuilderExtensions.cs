using ExcelReader.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ExcelReader.ConsoleApp.Installers;

/// <summary>
/// Microsoft.Extensions.Hosting.IHostBuilder interface extension methods.
/// </summary>
public static class IHostBuilderExtensions
{
    #region Methods

    public static void AddDependancies(this IHostBuilder builder)
    {
        builder.ConfigureServices((hostContext, services) =>
        {
            services.RegisterServices();

            // Configure Logging.
            services.AddSerilog((service, config) => config.ReadFrom.Configuration(hostContext.Configuration).Enrich.FromLogContext().WriteTo.Console());

            // Configure Options.
            services.AddOptions<ApplicationOptions>().Bind(hostContext.Configuration.GetSection(nameof(ApplicationOptions)));
        });
    }

    #endregion
}
