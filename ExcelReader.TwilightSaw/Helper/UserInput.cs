using Spectre.Console;
using System.Text.RegularExpressions;

namespace ExcelReader.TwilightSaw.Helper;

public class UserInput
{
    public static string CreateRegex(string regexString, string messageStart, string messageError)
    {
        var regex = new Regex(regexString);
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>($"[green]{messageStart}[/]")
                .Validate(value => regex.IsMatch(value)
                    ? ValidationResult.Success()
                    : ValidationResult.Error($"[red]{messageError}[/]")));
        Console.Clear();
        return input;
    }

    public static string Create(string messageStart)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>($"[green]{messageStart}[/]")
                .AllowEmpty());
        Console.Clear();
        return input;
    }

    public static string CreateWithSample(string messageStart, string previousText)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>($"{messageStart}")
                .DefaultValue($"{previousText}")
                .AllowEmpty());
    }

    public static string CreateChoosingList(List<string> variants, string backVariant)
    {
        variants.Add(backVariant);
        return AnsiConsole.Prompt(new SelectionPrompt<string>()
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