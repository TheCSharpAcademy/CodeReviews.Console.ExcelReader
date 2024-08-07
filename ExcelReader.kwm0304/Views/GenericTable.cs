using Spectre.Console;

namespace ExcelReader.kwm0304.Views;

public class GenericTable
{
  public string Title { get; set; }
  public List<Dictionary<string, object>> Rows { get; set; }
  public List<string> Columns { get; set; }
  public GenericTable(string title, List<string> columns, List<Dictionary<string, object>> rows)
  {
    Title = title;
    Columns = columns;
    Rows = rows;
  }
  public void Show()
  {
    var table = new Table()
    .Title(Title)
    .Centered()
    .Border(TableBorder.Markdown)
    .BorderStyle(new Style(foreground: Color.DarkCyan, decoration: Decoration.Bold));

    table.AddColumns(Columns.ToArray());
    foreach (var row in Rows)
    {
      var rowValues = Columns.Select(col => row.TryGetValue(col, out object? value) ? value?.ToString() ?? "" : "").ToArray();
      table.AddRow(rowValues);
    }
    AnsiConsole.Write(table);
  }
}