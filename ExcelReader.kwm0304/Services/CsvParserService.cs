using ExcelReader.kwm0304.Models;
using OfficeOpenXml;
using Spectre.Console;

namespace ExcelReader.kwm0304.Services;

public class CsvParserService
{
  public Response<Dictionary<string, object>> ParseCsvFromWorkbook(FileInfo info, int colRow, int dataRow)
  {
    var response = new Response<Dictionary<string, object>>();
    string header = info.Name;
    using ExcelPackage package = new(info);
    if (package.Workbook.Worksheets.Count < 1)
    {
      AnsiConsole.WriteLine("No workbook present");
      return new Response<Dictionary<string, object>>();
    }
    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

    if (worksheet == null)
    {
      AnsiConsole.WriteLine("No workbook present");
      return new Response<Dictionary<string, object>>();
    }
    response.ColumnNames = worksheet.Cells[colRow, 1, colRow, worksheet.Dimension.End.Column]
                .Select(cell => cell.Text)
                .ToList();
    response.RowValues = [];
    for (int row = dataRow; row <= worksheet.Dimension.End.Row; row++)
    {
      var rowData = new Dictionary<string, object>();
      for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
      {
        var cell = worksheet.Cells[row, col];
        string colName = response.ColumnNames[col - 1];
        rowData[colName] = GetTypedCellValue(cell);
      }
      response.RowValues.Add(rowData);
    }
    return response;
  }

  public Response<Dictionary<string, object>> ParseCsv(FileInfo info, int colRow, int dataRow)
  {
    var response = new Response<Dictionary<string, object>>();
    string header = info.Name;

    string[] lines = File.ReadAllLines(info.FullName);

    if (lines.Length < dataRow)
    {
      throw new InvalidOperationException("The CSV file doesn't have enough rows.");
    }

    response.ColumnNames = ParseCsvLine(lines[colRow - 1]);

    response.RowValues = new List<Dictionary<string, object>>();
    for (int i = dataRow - 1; i < lines.Length; i++)
    {
      var rowData = new Dictionary<string, object>();
      var values = ParseCsvLine(lines[i]);

      for (int j = 0; j < Math.Min(response.ColumnNames.Count, values.Count); j++)
      {
        string colName = response.ColumnNames[j];
        rowData[colName] = ParseValue(values[j]);
      }

      response.RowValues.Add(rowData);
    }

    return response;
  }
  private List<string> ParseCsvLine(string line)
  {
    return line.Split(',').Select(value => value.Trim()).ToList();
  }
  private object ParseValue(string value)
  {
    // Try parsing as different types
    if (int.TryParse(value, out int intValue))
      return intValue;
    if (double.TryParse(value, out double doubleValue))
      return doubleValue;
    if (DateTime.TryParse(value, out DateTime dateValue))
      return dateValue;
    if (bool.TryParse(value, out bool boolValue))
      return boolValue;

    // If all else fails, return as string
    return value;
  }

  private object GetTypedCellValue(ExcelRange cell)
  {
    return cell.Value switch
    {
      null => null!,
      double d when cell.Style.Numberformat.Format.Contains('%') => d,
      double d => d,
      int i => i,
      bool b => b,
      DateTime dt => dt,
      string s => s,
      _ => cell.Text,
    };
  }
}