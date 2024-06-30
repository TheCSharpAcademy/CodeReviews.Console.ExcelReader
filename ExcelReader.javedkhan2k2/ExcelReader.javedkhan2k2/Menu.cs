using Spectre.Console;

namespace ExcelReader;

public class ExcelReaderMenu
{
    public static string CancelOperation = $"[maroon]Go Back[/]";

    public string[] MainMenu = ["Read Excel File", "Read CSV file", "Read Doc File", "Exit"];
    public string[] ExcelMenu = ["Export as CSV", "Export as PDF", "Exit"];
    public string[] CsvMenu = ["Export as Excel", "Export as PDF", "Exit"];
    public string Title = "[yellow]Please Select An [blue]Action[/] From The Options Below[/]";
    public string GetMainMenu()
    {
        return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(Title)
                    .PageSize(10)
                    .AddChoices(MainMenu)
        );
    }

    public string GetExcelMenu()
    {
        return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(Title)
                    .PageSize(10)
                    .AddChoices(ExcelMenu)
        );
    }

    public string GetCsvMenu()
    {
        return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(Title)
                    .PageSize(10)
                    .AddChoices(CsvMenu)
        );
    }

}