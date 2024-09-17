using ExcelReader.Models;
using Spectre.Console;

namespace ExcelReader.Utilities;
public class Logger
{
    internal static void Log(string message)
    {
        AnsiConsole.MarkupLine($"[bold][green]{message}[/][/]");
    }

    internal static void DisplayData (List<DataModel> dataModels)
    {
        var table = new Table()
            .AddColumn("Date")
            .AddColumn("League")
            .AddColumn("Home")
            .AddColumn("Away")
            .AddColumn("Home Probability")
            .AddColumn("Away Probability")
            .AddColumn("Over 2 Goals");

        foreach (var dataModel in dataModels)
        {
            table.AddRow(dataModel.Date, dataModel.League, dataModel.Home, dataModel.Away, dataModel.HomeProbability, dataModel.AwayProbability, dataModel.OverTwoGoals);
        }

        AnsiConsole.Write(table);
    }
}