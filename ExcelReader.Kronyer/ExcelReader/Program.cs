using ExcelReader.Repositories;
using ExcelReader.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExcelReader;

internal class ExcelReader
{
    internal static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
    .AddSingleton<PersonContext>()
    .AddSingleton<IPersonRepository, PersonRepository>()
    .AddSingleton<Services>()
    .BuildServiceProvider();

        var services = serviceProvider.GetRequiredService<Services>();

        services.DropTable();
        services.ReadExcel();
        services.PrintTable();
    }
}

