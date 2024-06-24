using ExcelReader.samggannon.Controllers;
using ExcelReader.samggannon.Models;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using System.Diagnostics;

namespace ExcelReader.samggannon.UI;

internal static class ConsoleOutput
{
    // this feels out of place, i forgot why i put this here.
    internal static async Task EnsuredatabaseDeletion(IServiceProvider serviceProvider)
    {
        DbPlayerController controller = serviceProvider.GetService<DbPlayerController>();
        await controller.EnsureDelete();
    }

    internal static async Task EnsureDatabaseCreated(IServiceProvider serviceProvider)
    {
        DbPlayerController controller = serviceProvider.GetService<DbPlayerController>();
        await controller.EnsureCreate();
    }

    // why do i have an an extension method for console writeline??
    internal static void InformUser(string v)
    {
        Console.WriteLine(v);
    }

    internal static void ShowTable(List<Player> playerTableData)
    {
        throw new NotImplementedException();
    }
}
