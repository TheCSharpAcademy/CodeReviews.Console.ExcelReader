using ExcelReader.samggannon.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace ExcelReader.samggannon.UI;

internal static class ConsoleOutput
{
    // this feels out of place, i forgot why i put this here.
    internal static async Task EnsuredatabaseDeletion(IServiceProvider serviceProvider)
    {
        DbPlayerConroller controller = serviceProvider.GetService<DbPlayerConroller>();
        await controller.EnsureDelete();
    }

    // why do i have an an extension method for console writeline??
    internal static void InformUser(string v)
    {
        Console.WriteLine(v);
    }
}
