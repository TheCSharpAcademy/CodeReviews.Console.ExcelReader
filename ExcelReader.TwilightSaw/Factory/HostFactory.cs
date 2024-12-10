using ExcelReader.TwilightSaw.Controller;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExcelReader.TwilightSaw.Factory;

internal class HostFactory
{
    public static IHost CreateDbHost(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;
                services.AddSingleton<DbController>();
                services.AddSingleton<ReaderController>();
            }).Build();
    }

}