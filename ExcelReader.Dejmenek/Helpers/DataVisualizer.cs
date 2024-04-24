using ExcelReader.Dejmenek.Models;
using Spectre.Console;

namespace ExcelReader.Dejmenek.Helpers;
public class DataVisualizer
{
    public static void ShowItems(List<Item> items)
    {
        var table = new Table().Title("ITEMS");

        table.AddColumn("Id");
        table.AddColumn("Name");
        table.AddColumn("Quantity");
        table.AddColumn("UnitPrice");

        foreach (var item in items)
        {
            table.AddRow(item.Id.ToString(), item.Name, item.Quantity.ToString(), item.UnitPrice.ToString("0.00"));
        }

        AnsiConsole.Write(table);
    }
}
