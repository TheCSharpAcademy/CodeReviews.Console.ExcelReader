
using Excel_Reader.Lawang;
using Excel_Reader.Lawang.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = CreateDefaultHostBuilder(args).Build();

using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

await services.GetRequiredService<App>().Run();




static IHostBuilder CreateDefaultHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args).ConfigureServices((_, services) =>
    {
        services.AddSingleton<App>();
        services.AddSingleton<Database>();
        services.AddSingleton<ExcelOperator>();
        services.AddSingleton<FileReader>();
    });
}







































