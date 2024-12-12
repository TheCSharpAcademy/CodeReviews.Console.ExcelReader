using System;
using Spectre.Console;

namespace ExcelReader.mefdev.Controllers;

public class ExcelBase
{
    public ExcelBase()
    {

    }

    protected void DisplayAllItems<T>(List<T> items)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.DodgerBlue3)
            .Width(250);

        var firstItem = items.FirstOrDefault();
        if (firstItem != null)
        {
            foreach (var prop in firstItem.GetType().GetProperties())
            {
                table.AddColumn(new TableColumn(prop.Name));
            }
        }
        if (items != null && items.Count() > 0)
        {
            foreach (var item in items)
            {
                var rowValues = item.GetType().GetProperties()
                    .Select(prop => prop.GetValue(item)?.ToString() ?? "N/A")
                    .ToArray();

                table.AddRow(rowValues);
            }
        }
        AnsiConsole.Write(table);
        AnsiConsole.Confirm("Press any key to continue... ");
    }

    protected void DisplayMessage(string message, string color = "yellow")
    {
        AnsiConsole.MarkupLine($"[{color}]{message}[/]");
    }
}


