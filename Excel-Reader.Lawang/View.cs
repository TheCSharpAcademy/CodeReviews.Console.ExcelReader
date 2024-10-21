using Excel_Reader.Lawang.Model;
using Spectre.Console;

namespace Excel_Reader.Lawang;

public static class View
{
    public static void RenderTitle(string title, Color textColor, Color borderColor, string header, string headerColor, BoxBorder borderStyle)
    {
        var panel = new Panel(new FigletText($"{title}").Color(textColor))
                   .BorderColor(borderColor)
                   .PadTop(1)
                   .PadBottom(1)
                   .Header(new PanelHeader($"[{headerColor} bold]{header}[/]"))
                   .Border(borderStyle)
                   .Expand();

        AnsiConsole.Write(panel);
    }
    public static void ShowInstruction()
    {
        var panel = new Panel(new Markup("[orange3 bold]Please enter a [green]\"Read\"[/] to read and seed the data into the database\nEnter [green]\"Write\"[/] to Write into the excel file[/]"))
                .Header("[bold cyan]Input[/]", Justify.Center)
                .Padding(1, 1, 1, 1)
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Blue3);

        // Render the panel
        AnsiConsole.Write(panel);
    }

    public static void DisplayWorkSheetNames(List<string> workSheet)
    {
        var rule = new Rule("[aquamarine1 bold]Name of all work sheet[/]").LeftJustified();
        AnsiConsole.Write(rule);
        int count = 1;
        foreach (var name in workSheet)
        {
            AnsiConsole.MarkupLine($"[blue bold]{count}. {name}[/] :page_facing_up:");
            count++;
        }
    }

    public static void DisplayKnownExcel(List<Person> people)
    {
        if (people.Count() == 0)
        {
            Panel panel = new Panel(new Markup("[red bold]CONTACT IS EMPTY![/]"))
                .Border(BoxBorder.Heavy)
                .BorderColor(Color.IndianRed1_1)
                .Padding(1, 1, 1, 1)
                .Header("Result");

            AnsiConsole.Write(panel);
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .Expand()
            .BorderColor(Color.Aqua)
            .ShowRowSeparators();

        table.AddColumns(new TableColumn[]
        {
           new TableColumn("[darkgreen bold]Id[/]").Centered(),
           new TableColumn("[darkcyan bold]First Name [/]").Centered(),
           new TableColumn("[darkcyan bold]Last Name [/]").Centered(),
           new TableColumn("[darkcyan bold]Gender[/]").Centered(),
           new TableColumn("[darkgreen bold]Country[/]").Centered(),
           new TableColumn("[darkgreen bold]Age[/]").Centered(),
           new TableColumn("[darkgreen bold]Date[/]").Centered()
        });

        foreach (var person in people)
        {
            table.AddRow(
                new Markup($"[cyan1]{person.Id}[/]").Centered(),
                new Markup($"[turquoise2]{person.FirstName}[/]").Centered(),
                new Markup($"[turquoise2]{person.LastName}[/]").Centered(),
                new Markup($"[yellow]{person.Gender}[/]").Centered(),
                new Markup($"[turquoise2]{person.Country}[/]").Centered(),
                new Markup($"[turquoise2]{person.Age}[/]").Centered(),
                new Markup($"[turquoise2]{person.Date}[/]").Centered()

            );
        }
        Console.WriteLine();
        var rule = new Rule("[aquamarine1 bold]CREATED DATA FROM KNOWN EXCEL FILE[/]").LeftJustified();
        AnsiConsole.Write(rule);
        
        AnsiConsole.Write(table);
    }

     public static void RenderTable(List<string[]> data)
    {
        if (data.Count() == 0)
        {
            Panel panel = new Panel(new Markup("[red bold]CSV IS EMPTY![/]"))
                .Border(BoxBorder.Heavy)
                .BorderColor(Color.IndianRed1_1)
                .Padding(1, 1, 1, 1)
                .Header("Result");

            AnsiConsole.Write(panel);
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .Expand()
            .BorderColor(Color.Aqua)
            .ShowRowSeparators();


        TableColumn[] columnNames = new TableColumn[data[0].Length];
        string[] headers = data[0];
        for(int i = 0; i < columnNames.Length; i++)
        {
            columnNames[i] = new TableColumn($"[darkgreen bold]{headers[i]}[/]").Centered();
        }
        table.AddColumns(columnNames);

        for(int i = 1; i < data.Count(); i++)
        {
            List<Markup> rowMarkup = new List<Markup>();
            foreach(string s in data[i])
            {
                rowMarkup.Add(new Markup($"[turquoise2]{s}[/]").Centered());
            }
            table.AddRow(rowMarkup);
        }

        AnsiConsole.Write(table);
    }
}
