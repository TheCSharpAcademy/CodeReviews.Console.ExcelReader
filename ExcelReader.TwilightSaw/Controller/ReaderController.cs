using OfficeOpenXml;

namespace ExcelReader.TwilightSaw.Controller;

public class ReaderController
{
    private readonly string _filePath = @"C:\Users\Alex\source\repos\projects\CodeReviews.Console.ExcelReader\ExcelReader.TwilightSaw\db.xlsx";

    public (List<List<string>> values, string name) Read()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(new FileInfo(_filePath));

        var worksheet = package.Workbook.Worksheets[0];
        var rowCount = worksheet.Dimension.Rows;
        var colCount = worksheet.Dimension.Columns;

        var stringList = new List<List<string>>();

        for (var i = 1; i <= rowCount; i++)
        {
            var rowList = new List<string>();
            for (var j = 1; j <= colCount; j++)
                rowList.Add(worksheet.Cells[i, j].Value.ToString());
            stringList.Add(rowList);
        }
        return (values: stringList, name: worksheet.Name);
    }
}