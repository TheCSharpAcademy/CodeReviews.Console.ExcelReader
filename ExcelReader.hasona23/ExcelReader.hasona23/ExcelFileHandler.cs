using OfficeOpenXml;

namespace ExcelReader.hasona23;

public class ExcelFileHandler(string filePath, string sheetName)
{
    public ExcelWorksheet Worksheet { get; } = new ExcelPackage(filePath).Workbook.Worksheets[sheetName];

    public int ColumnCount => Worksheet.Columns.Count();
    public int RowCount => Worksheet.Rows.Count();
}