using Spectre.Console;

namespace ExcelReader.TwilightSaw.Helper;

public class Validation
{
    public static string Validate(Action action, bool getMessage)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            return getMessage ? e.Message : "";
        }
        return "Executed successfully";
    }

    public static bool Validate<T>(T action, out T back)
    {
        try
        {
            back = action;
        }
        catch (Exception e)
        {
            back = default;
            AnsiConsole.MarkupLine($"[olive]{e.Message}[/]");
            return false;
        }
        return true;
    }

    public static void EndMessage(string? message)
    {
        if (message != null)
        {
            AnsiConsole.MarkupLine($"[olive]{message}[/]");
            AnsiConsole.Markup($"[grey]Press any key to continue.[/]");
            Console.ReadKey(intercept: true);
        }
        Console.Clear();
    }
}