using Spectre.Console;

namespace ExcelReader;

public static class Prompter
{
    public static T EnumPrompt<T>() where T : struct, Enum
    {
        return AnsiConsole.Prompt<T>(
            new SelectionPrompt<T>()
                .Title("Choose an option: ")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(Enum.GetValues<T>()).UseConverter<T>(EnumHelper.GetTitle)
        );
    }

    public static void PressKeyToContinuePrompt()
    {
        AnsiConsole.Write("Press any key to continue...");
        Console.ReadKey();
    }
}