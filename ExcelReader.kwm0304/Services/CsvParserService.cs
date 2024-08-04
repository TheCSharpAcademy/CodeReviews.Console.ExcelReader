using ExcelReader.kwm0304.Models;
using OfficeOpenXml;

namespace ExcelReader.kwm0304.Services;

public class CsvParserService
{
  public Response<Dictionary<string, object>> ParseCsv(FileInfo info, int colRow, int dataRow)
  {
    var response = new Response<Dictionary<string, object>>();
    string header = info.Name;
    using ExcelPackage package = new(info);
    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

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