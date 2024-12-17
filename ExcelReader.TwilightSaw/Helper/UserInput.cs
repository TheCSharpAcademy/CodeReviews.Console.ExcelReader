using Spectre.Console;
using OfficeOpenXml;

namespace ExcelReader.TwilightSaw.Helper;

public class UserInput
{
    public static string Create(string messageStart)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>($"[green]{messageStart}[/]")
                .AllowEmpty());
        Console.Clear();
        return input;
    }

    public static ExcelRangeBase CreateChoosingList(List<ExcelRangeBase> variants)
    {
        return AnsiConsole.Prompt(new SelectionPrompt<ExcelRangeBase>()
            .Title("[blue]Please, choose an option from the list below:[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more categories[/]")
            .UseConverter(r => $"{r.Value}")
            .AddChoices(variants));
    }

    public static T CreateChoosingList<T>(List<T> variants)
    {
        return AnsiConsole.Prompt(new SelectionPrompt<T>()
            .Title("[blue]Please, choose an option from the list below:[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more categories[/]")
            .AddChoices(variants));
    }

    public static bool CreateWithConfirm(string message)
    {
       return AnsiConsole.Prompt(
            new TextPrompt<bool>($"[blue]{message}[/]")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(true)
                .WithConverter(choice => choice ? "y" : "n"));
    }
}