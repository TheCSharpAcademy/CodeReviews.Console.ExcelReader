using ExcelReader.Controllers;
using Spectre.Console;

namespace ExcelReader.Views;

public static class View
{
    public static void DisplayData(List<List<string>> rows, List<string> cols)
    {
        var table = new Table();

        table.AddColumns(cols.ToArray());

        foreach (var row in rows)
        {
            table.AddRow(row.ToArray());
        }

        AnsiConsole.Write(table);
    }

    public static (string path, bool hasHeader) PromptForFile()
    {
        AnsiConsole.WriteLine("\n");

        var defaultPath = Path.GetFullPath("./Data/data.xlsx");
        var path = AnsiConsole.Ask(
            "[blue]Enter path to excel file?[/] [grey](Press enter to use default)[/]\n",
            defaultPath
        );

        AnsiConsole.WriteLine("\n");
        var hasHeader = AnsiConsole.Confirm("[blue]Does file have a header?[/]", true);

        AnsiConsole.WriteLine(path);

        if (!File.Exists(path))
        {
            AnsiConsole.MarkupLine("[red]File could not be found. Exiting ...[/]");
            Environment.Exit(1);
        }

        return (path, hasHeader);
    }
}