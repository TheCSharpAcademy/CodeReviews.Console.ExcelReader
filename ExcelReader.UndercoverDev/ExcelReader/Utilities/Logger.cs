using Spectre.Console;

namespace ExcelReader.Utilities;
public class Logger
{
    internal static void Log(string message)
    {
        AnsiConsole.MarkupLine($"[bold][green]message[/][/]");
    }
}