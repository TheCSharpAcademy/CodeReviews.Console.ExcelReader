using Spectre.Console;
using static ExcelReader.K_MYR.Enums;

namespace ExcelReader.K_MYR;

internal class UserInterface
{
    private readonly DataReader _DataReader;

    public UserInterface(DataReader dataReader)
    {
        _DataReader = dataReader;
    }

    public void ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.Write(new Panel("[springgreen2_1]Excel Data Reader[/]").BorderColor(Color.DarkOrange3_1));

            var selection = AnsiConsole.Prompt(new SelectionPrompt<MainMenuOptions>()
                                                    .AddChoices(Enum.GetValues(typeof(MainMenuOptions)).Cast<MainMenuOptions>()));

            switch (selection)
            {
                case MainMenuOptions.ReadExcelFile:
                    ViewData();
                    break;
                case MainMenuOptions.Exit:
                    Environment.Exit(0);
                    break;
            }
        }
    }

    private void ViewData()
    {
        var path = GetFilePath();

        var data = _DataReader.ReadFromXlsx(path);

        var table = new Table()
                        .AddColumn("#")
                        .BorderColor(Color.DarkOrange3_1);

        foreach (var col in _DataReader.Repo.Columns)
        {
            table.AddColumn($"[springgreen2_1]{col}[/]");
        }

        if (data is not null)
        {
            for (int i = 0; i < data.Rows.Count; i++)
            {
                var values = new List<string>()
                {
                    i.ToString(),
                };

                for (int j = 1; j < _DataReader.Repo.Columns.Count + 1; j++)
                {
                    values.Add(data.Rows[i][j]?.ToString() ?? "");
                }

                table.AddRow(values.ToArray());
            }
        }

        Console.Clear();
        AnsiConsole.Write(table);
        Console.ReadKey();
    }

    private FileInfo GetFilePath()
    {
        string result = AnsiConsole.Ask<string>("[springgreen2_1]Please Enter A File Path: [/]");

        while (!File.Exists(result))
        {
            AnsiConsole.Write(new Panel("[red]Invalid Input: File Does Not Exist[/]").BorderColor(Color.Red));
            result = AnsiConsole.Ask<string>("Please Enter A File Path: ");
        }

        return new FileInfo(result);
    }
}
