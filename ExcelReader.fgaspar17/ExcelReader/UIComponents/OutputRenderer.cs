using Spectre.Console;

namespace ExcelReader;

public static class OutputRenderer
{
    /// <summary>
    /// Displays a list of objects in a table format.
    /// </summary>
    public static void ShowTable(Dictionary<string, List<string>> columnValues, string title)
    {
        // Create a table
        var table = new Table();

        // Set the table border and style options
        table.Border(TableBorder.Rounded);
        table.BorderColor(Color.LightCoral);
        table.Title($"[bold yellow]{title}[/]");
        table.Caption("Generated on " + DateTime.Now.ToString("g"));


        // Add columns
        foreach (string column in columnValues.Keys)
        {
            table.AddColumn(new TableColumn(new Markup("[bold yellow]" + column + "[/]")).Centered().PadRight(2));
        }

        var firstRow = columnValues.Values.FirstOrDefault();
        if (firstRow == null) return;
        int rowCount = firstRow.Count;
        // Add rows
        for (int i = 0; i < rowCount; i++)
        {
            List<string> row = new();

            foreach (string column in columnValues.Keys)
            {
                row.Add(columnValues[column][i] ?? "N/A");
            }
            table.AddRow(row.ToArray());
        }

        AnsiConsole.Write(table);
    }

    public static void ShowTable(List<Dictionary<string, string>> rows, string title)
    {
        if (rows.Count == 0) return;

        // Create a table
        var table = new Table();

        // Set the table border and style options
        table.Border(TableBorder.Rounded);
        table.BorderColor(Color.LightCoral);
        table.Title($"[bold yellow]{title}[/]");
        table.Caption("Generated on " + DateTime.Now.ToString("g"));


        // Add columns
        foreach (var column in rows[0].Keys)
        {
            table.AddColumn(new TableColumn(new Markup("[bold yellow]" + column + "[/]")).Centered().PadRight(2));
        }


        // Add rows
        foreach (var row in rows)
        {
            table.AddRow(row.Values.ToArray());
        }

        AnsiConsole.Write(table);
    }
}