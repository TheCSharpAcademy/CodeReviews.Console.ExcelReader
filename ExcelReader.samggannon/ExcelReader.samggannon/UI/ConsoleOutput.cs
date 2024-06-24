using ExcelReader.samggannon.Controllers;
using ExcelReader.samggannon.Models;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using System.Diagnostics;

namespace ExcelReader.samggannon.UI;

internal static class ConsoleOutput
{
   
    // why do i have an an extension method for console writeline??
    internal static void InformUser(string v)
    {
        Console.WriteLine(v);
    }

    internal static void ShowTable(List<Player> playerTableData)
    {
        // add ansi console to display a table and show the data
        throw new NotImplementedException();
    }
}
