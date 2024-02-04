using Spectre.Console;
using System.Diagnostics;

namespace ExcelReader.Doc415;

internal class ImportControl
{
    public void Import()
    {
        var excelFile = FileHandler.SelectFile();
        Stopwatch sw = Stopwatch.StartNew();
        var Importer = new ImportExcelService();
        Importer.ImportExcelToDb(excelFile);
        sw.Stop();
        Console.WriteLine($"Import completed in {sw.ElapsedMilliseconds} miliseconds.");
        var DbData = Importer.GetDbData();
        ShowDb(DbData);
    }

    public void ShowDb(List<List<string>> data)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumns(ExcelFileHandler._colNames.ToArray());
        foreach (var row in data)
        {
            table.AddRow(row.ToArray());
        }
        AnsiConsole.Write(table);
    }
}
