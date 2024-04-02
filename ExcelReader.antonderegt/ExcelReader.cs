using OfficeOpenXml;

namespace ExcelReader;

public class ExcelFileRead
{
    public static List<Number> ReadXLS(string FilePath)
    {
        FileInfo existingFile = new(FilePath);
        using ExcelPackage package = new(existingFile);
        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        int colCount = worksheet.Dimension.End.Column;
        int rowCount = worksheet.Dimension.End.Row;
        List<Number> excelRows = [];

        // Start at two because we can skip the header row
        for (int row = 2; row <= rowCount; row++)
        {
            int digits = -1;
            _ = int.TryParse(worksheet.Cells[row, 1].Value?.ToString()?.Trim(), out digits);
            string name = worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty;

            Number dynamicExcelRow = new()
            {
                Digits = digits,
                Name = name
            };

            excelRows.Add(dynamicExcelRow);
        }

        return excelRows;
    }
}