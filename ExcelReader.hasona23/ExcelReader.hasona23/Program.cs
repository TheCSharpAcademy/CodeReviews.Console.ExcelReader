using ExcelReader.hasona23;
using Spectre.Console;
using Spectre.Console.Rendering;

ExcelFileHandler excel = new("BikeStore.xlsx","data");
Database db = new Database(excel);
db.TransferData();
var data = db.GetData();
Console.WriteLine("Displaying Table...");
Table t = new Table().Title("BikeStore");
t.Alignment(Justify.Center);
t.Expand();
t.BorderStyle(new Style(Color.Yellow));
t.AddColumns("[darkturquoise]BikeId[/]","[darkturquoise]Brand[/]","[darkturquoise]Model[/]","[darkturquoise]Price[/]"
    ,"[darkturquoise]Color[/]","[darkturquoise]Size[/]","[darkturquoise]Weight[/]");
t.Border(new HeavyTableBorder());
foreach (var row in data)
{
    t.AddRow($"[white]{row[0]}[/]",$"[white]{row[1]}[/]",$"[white]{row[2]}[/]",$"[white]{row[3]}[/]",$"[white]{row[4]}[/]",
        $"[white]{row[5]}[/]",$"[white]{row[6]}[/]");
}
AnsiConsole.Write(t);