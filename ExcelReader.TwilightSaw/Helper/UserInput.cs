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
}