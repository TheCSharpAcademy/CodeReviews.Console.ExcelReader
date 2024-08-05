using ExcelReader.kwm0304.Models;
using ExcelReader.kwm0304.Utils;
using OfficeOpenXml;
using Spectre.Console;

namespace ExcelReader.kwm0304.Services;

public class CsvParserService
{
  private readonly Validation _validation;
  public CsvParserService()
  {
    _validation = new();
  }
  public void ConvertCsvToXlsx(string csvFilePath, string xlsxFilePath)
  {
    var csvLines = File.ReadAllLines(csvFilePath);
    using var package = new ExcelPackage();
    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
    for (int rowIndex = 0; rowIndex < csvLines.Length; rowIndex++)
    {
      var row = csvLines[rowIndex].Split(';');
      for (int colIndex = 0; colIndex < row.Length; colIndex++)
      {
        worksheet.Cells[rowIndex + 1, colIndex + 1].Value = row[colIndex];
      }
    }
    File.WriteAllBytes(xlsxFilePath, package.GetAsByteArray());
  }

  public Response<List<Dictionary<string, object>>> ParseCsvFromWorkbook(FileInfo info, int colRow, int dataRow)
  {
    var response = new Response<List<Dictionary<string, object>>>();
    using var package = new ExcelPackage(info);
    if (package.Workbook.Worksheets.Count < 1)
    {
      throw new InvalidDataException("No workbook present");
    }
    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
    response.ColumnNames = worksheet.Cells[colRow, 1, colRow, worksheet.Dimension.End.Column]
        .Select(cell => _validation.NormalizeColumnName(cell.Text))
        .ToList();
    response.Header = "Header";
    response.RowValues = [];
    for (int row = dataRow; row <= worksheet.Dimension.End.Row; row++)
    {
      var rowData = new Dictionary<string, object>();
      for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
      {
        var cell = worksheet.Cells[row, col];
        string colName = response.ColumnNames[col - 1];
        rowData[colName] = _validation.GetTypedCellValue(cell);
      }
      response.RowValues.Add(rowData);
    }
    return response;
  }
}