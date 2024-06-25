using ExcelReader.samggannon.Models;
using Spectre.Console;

namespace ExcelReader.samggannon.UI;

internal static class ConsoleOutput
{
    internal static void ShowTable(List<Player> playerTableData)
    {
        var table = new Table();
        table.AddColumn("Year");
        table.AddColumn("Team");
        table.AddColumn("Name");
        table.AddColumn("Number");
        table.AddColumn("Position");
        table.AddColumn("Height");
        table.AddColumn("Weight");
        table.AddColumn("Age");
        table.AddColumn("Experience");
        table.AddColumn("College");

        foreach (Player player in playerTableData)
        {
            table.AddRow(
                (player.Year?.ToString() ?? "-"),
                (player.Team?.ToString() ?? "-"),
                (player.Name?.ToString() ?? "-"),
                (player.Number?.ToString() ?? "-"),
                (player.Pos?.ToString() ?? "-"),
                (player.Height?.ToString() ?? "-"),
                (player.Weight?.ToString() ?? "-"),
                (player.Age?.ToString() ?? "-"),
                (player.Experience?.ToString() ?? "-"),
                (player.College?.ToString() ?? "-")
            );
        }

        Console.WriteLine();
        AnsiConsole.Write(table);
        Console.ReadLine();
        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();
    }
}


