using OfficeOpenXml;

namespace ExcelReader.wkktoria;

public static class Program
{
    public static void Main()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        Console.Write("Enter path to .xlsx file: ");
        var filePath = Console.ReadLine();

        if (!new FileInfo(filePath!).Exists)
        {
            Console.WriteLine("File doesn't exist!");
            return;
        }

        var file = new FileInfo(filePath!);

        using var package = new ExcelPackage(file);

        var worksheet = package.Workbook.Worksheets[0];
        var colCount = worksheet.Dimension.End.Column;
        var rowCount = worksheet.Dimension.End.Row;

        for (var row = 1; row < rowCount; row++)
        for (var col = 1; col < colCount; col++)
            Console.WriteLine($"Row: {row} Column: {col} Value: {worksheet.Cells[row, col].Value?.ToString()!.Trim()}");
    }
}