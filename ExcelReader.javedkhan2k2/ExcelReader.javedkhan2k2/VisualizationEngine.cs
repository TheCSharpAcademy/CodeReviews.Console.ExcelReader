using System.Diagnostics.CodeAnalysis;
using ExcelReader.Entities;
using Spectre.Console;
namespace Exercisetacker.UI;

internal class VisualizationEngine
{

    internal static void DisplayContinueMessage()
    {
        AnsiConsole.Markup($"\n[yellow]Press [blue]Enter[/] To Continue[/]\n");
        Console.ReadLine();
    }

    public static Table CreateTable(string title, string footer)
    {
        var table = new Table();
        table.Title($"[yellow]{title}[/]");
        table.Caption(footer);
        table.Border = TableBorder.Square;
        table.ShowRowSeparators = true;
        return table;
    }

    internal static void DisplayCancelOperation()
    {
        AnsiConsole.Markup("[maroon]The Operation is Canceled by the User.[/]\n");
        DisplayContinueMessage();
    }

    internal static void DisplayFailureMessage(string message)
    {
        AnsiConsole.Markup($"[maroon]{message}[/]");
        DisplayContinueMessage();
    }

    internal static void DisplaySuccessMessage(string message)
    {
        AnsiConsole.Markup($"[green]{message}[/]");
        DisplayContinueMessage();
    }

    internal static void DisplayDateTimeError()
    {
        AnsiConsole.Markup("[maroon]The End Date Time should be later than Start Date Time.[/]\n");
        DisplayContinueMessage();
    }

    internal static void DisplayAllContacts(List<Contact>? contacts, [AllowNull] string title)
    {
        if (title == null)
        {
            title = "";
        }
        var table = CreateTable(title, $"[yellow]Displaying [blue]{contacts.Count()}[/] records[/]");
        table.AddColumns(["[green]Id[/]", "[green]Name[/]", "[green]Email[/]", "[green]Phone Number[/]", "[green]Address[/]"]);
        foreach (var contact in contacts)
        {
            table.AddRow(contact.Id.ToString(), contact.Name, contact.Email, contact.PhoneNumber, contact.Address);
        }
        AnsiConsole.Write(table);
    }

}