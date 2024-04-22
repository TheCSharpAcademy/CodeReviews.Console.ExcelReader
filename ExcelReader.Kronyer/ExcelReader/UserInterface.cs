using Spectre.Console;

namespace ExcelReader;

internal class UserInterface
{
    public static void Loading(string loadMessage)
    {
        var spinner = AnsiConsole.Status().Spinner(Spinner.Known.Aesthetic).Start(loadMessage, ctx =>
        {
            int i = 0;
            Random random = new Random();
            while (i < 100)
            {
                i += random.Next(15, 33);
                if (i > 100)
                {
                    i = 100;
                }
                ctx.Status($"{loadMessage}{i}%");
                Thread.Sleep(150);
            }
            return true;
        });
    }
}
