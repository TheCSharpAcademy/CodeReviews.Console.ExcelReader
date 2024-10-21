
using System.Text;
using Excel_Reader.Lawang.Model;
using Microsoft.Extensions.Primitives;
using OfficeOpenXml;
using Spectre.Console;

namespace Excel_Reader.Lawang;

public class ExcelOperator
{

    public async Task<List<Person>> ReadExcel(FileInfo excelFile)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        List<Person> people = new();

        using var package = new ExcelPackage(excelFile);
        await package.LoadAsync(excelFile);

        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

        int rowCount = worksheet.Dimension.Rows;

        //Assumes the data row starts at row 2
        for (int i = 2; i <= rowCount; i++)
        {
            var person = new Person()
            {
                Id = int.Parse(worksheet.Cells[i, 1].Text),
                FirstName = worksheet.Cells[i, 2].Text,
                LastName = worksheet.Cells[i, 3].Text,
                Gender = worksheet.Cells[i, 4].Text,
                Country = worksheet.Cells[i, 5].Text,
                Age = int.Parse(worksheet.Cells[i, 6].Text),
                Date = worksheet.Cells[i, 7].Text
            };

            people.Add(person);
        }
        AnsiConsole.MarkupLine("[green bold]READING FROM EXCEL COMPLETE  :bookmark:[/]");
        return people;

    }

    public List<WorkSheet> ReadDynamicExcel(FileInfo fileInfo)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        List<WorkSheet> workSheets = new();
        using var package = new ExcelPackage(fileInfo);
        // loops through each worksheet in workbook
        foreach (ExcelWorksheet ws in package.Workbook.Worksheets)
        {
            int rowCount = ws.Dimension.End.Row;
            int colCount = ws.Dimension.End.Column;

            string tableName = ws.Name;

            var wsTable = BuildWorkSheetTable(ws, rowCount, colCount, tableName);
            workSheets.Add(wsTable);
        }
        AnsiConsole.MarkupLine("[green bold]READING FROM EXCEL COMPLETE  :bookmark:[/]");
        return workSheets;
    }

    private WorkSheet BuildWorkSheetTable(ExcelWorksheet ws, int rowCount, int colCount, string tableName)
    {
        string[] headers = new string[colCount];
        string[] tableValue = new string[rowCount - 1];

        // for collecting the header value
        for (int i = 1; i <= colCount; i++)
        {
            headers[i - 1] = ws.Cells[1, i].Text.Trim();
        }

        for (int i = 2; i <= rowCount; i++)
        {
            string value = RowValue(ws, i, colCount);
            tableValue[i - 2] = value;
        }

        return new WorkSheet()
        {
            Name = tableName,
            ColumnHeaders = headers,
            TableValue = tableValue
        };
    }

    private string RowValue(ExcelWorksheet ws, int start, int colCount)
    {
        var rowValue = new StringBuilder("(");
        for (int i = 1; i <= colCount; i++)
        {
            rowValue.Append($"'{ws.Cells[start, i].Text.Replace("\'", "")}',");
        }
        rowValue.Remove(rowValue.Length - 1, 1);
        rowValue.Append(")");
        return rowValue.ToString();
    }
    public void WriteIntoFile(FileInfo fileInfo)
    {
        Console.Clear();

        var confirmation = AnsiConsole.Prompt(new ConfirmationPrompt("[yellow bold]Do you want to write into Existing Worksheet?[/]"));
        if (confirmation)
        {
            WriteIntoExistingWorkSheet(fileInfo);
        }
        else
        {
            WriteIntoNewWorkSheet(fileInfo);
        }
    }

    private void WriteIntoNewWorkSheet(FileInfo fileInfo)
    {
        Console.Clear();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage(fileInfo);
        var rule = new Rule("[aquamarine1 bold]Create a new Worksheet[/]").LeftJustified();
        AnsiConsole.Write(rule);
        string workSheetName = AnsiConsole.Ask<string>("[green bold]\nGive the name of the new Worksheet: [/]");
        int columns = AnsiConsole.Ask<int>("[green bold]Enter the number of columns: [/]");

        if (columns == 0)
        {
            AnsiConsole.MarkupLine("[red bold]NUMBER OF COLUMNS CANNOT BE ZERO![/]");
            return;
        }
        var ws = package.Workbook.Worksheets.Add(workSheetName);

        Console.WriteLine();
        var col = new Rule("\n[aquamarine1 bold]Write Columns Name[/]").LeftJustified();
        AnsiConsole.Write(col);
        //Writing headers in the worksheet
        for (int i = 1; i <= columns; i++)
        {
            string header = AnsiConsole.Ask<string>($"[slateblue1 bold]Enter the name for column {i}: [/]");
            ws.Cells[1, i].Value = header;
        }
        Console.WriteLine();
        var row = new Rule("[yellow3 bold]\nWrite rows Value [/]").LeftJustified();
        AnsiConsole.Write(row);
        do
        {
            EnterValue(ws);
        } while (AnsiConsole.Prompt(new ConfirmationPrompt("[royalblue1 bold]\nDo you want to enter more rows in worksheet? [/]")));

        package.Save();

        AnsiConsole.MarkupLine("[green bold]\nDATA SAVED TO FILE!\n[/]");
    }
    private void WriteIntoExistingWorkSheet(FileInfo fileInfo)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(fileInfo);

        bool exit = false;

        var workSheetNames = package.Workbook.Worksheets.Select(ws => ws.Name).ToList();
        while (!exit)
        {
            Console.Clear();
            View.DisplayWorkSheetNames(workSheetNames);
            string workSheetName = AnsiConsole.Ask<string>("[blue bold]\nEnter the name of the WorkSheet: [/]");
            var ws = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name.Equals(workSheetName));

            if (ws != null)
            {
                EnterValue(ws);
                package.Save();
                AnsiConsole.MarkupLine("[green bold]DATA SAVED TO FILE!\n[/]");
                exit = AnsiConsole.Prompt(
                    new ConfirmationPrompt("[royalblue1 bold]\nDo you want to exit?[/]")
                );

            }
            else
            {
                exit = AnsiConsole.Prompt(new ConfirmationPrompt("[royalblue1 bold]\nDo you want to enter the worksheet name againg?\n[/]"));
            }
        }

    }

    private void EnterValue(ExcelWorksheet ws)
    {
        int colCount = ws.Dimension.Columns;
        string[] colNames = new string[colCount];
        for (int i = 1; i <= colCount; i++)
        {
            colNames[i - 1] = ws.Cells[1, i].Text;
        }

        int lastRow = ws.Dimension.End.Row + 1;

        for (int i = 1; i <= colCount; i++)
        {
            var value = AnsiConsole.Ask<string>($"[yellow bold]Enter the value for {colNames[i - 1]}: [/]");
            ws.Cells[lastRow, i].Value = value;
        }

    }


}
