using ExcelReader.TwilightSaw.Helper;
using ExcelReader.TwilightSaw.Model;
using OfficeOpenXml;
using Spectre.Console;

namespace ExcelReader.TwilightSaw.Reader;

public class ReaderXlsx(string filePath) : IReader
{
    public ReaderItem Read()
    {
        try
        {
            Console.WriteLine("Reading from the excel file...");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(new FileInfo(filePath));

            var worksheets = package.Workbook.Worksheets;
            var worksheetList = new List<(List<List<string>>, string)>();
            foreach (var worksheet in worksheets)
            {
                var rowCount = worksheet.Dimension.Rows;
                var colCount = worksheet.Dimension.Columns;
                var worksheetRows = new List<List<string>>();

                for (var i = 1; i <= rowCount; i++)
                {
                    var rowValues = new List<string>();
                    for (var j = 1; j <= colCount; j++)
                        rowValues.Add(worksheet.Cells[i, j].Value?.ToString() ?? "");
                    worksheetRows.Add(rowValues);
                }

                worksheetList.Add((worksheetRows, worksheet.Name));
            }

            return new ReaderItem(worksheetList, Path.GetFileNameWithoutExtension(filePath));
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public void Write()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(new FileInfo(filePath));

        var worksheets = package.Workbook.Worksheets;
        var list = worksheets.ToList();
        var sheet = UserInput.CreateChoosingList(list);
        var range = new List<ExcelRangeBase>();

        AnsiConsole.Write(new Rule($"[olive]{sheet.Name}[/]"));
        for (var i = 1; i <= sheet.Dimension.Columns; i++) range.Add(sheet.Cells[1, i]);
        var column = UserInput.CreateChoosingList(range.ToList());
        var emptyCellRow = sheet.Dimension.Rows + 1;

        AnsiConsole.Write(new Rule($"[olive]{column.Value}[/]"));
        var inputRead = UserInput.Create("Input new value into the column: ");
        for (var i = 1; i <= sheet.Dimension.Rows; i++)
        {
            var currentCell = sheet.Cells[i, column.Start.Column];
            if (string.IsNullOrEmpty(currentCell.Text)) emptyCellRow = i;
        }
        Console.Clear();

        sheet.Cells[emptyCellRow, column.Start.Column].Value = inputRead;
        var file = new FileInfo(filePath);
        package.SaveAs(file);
        AnsiConsole.MarkupLine("[olive]Sheet was edited successfully.[/]");
    }
}