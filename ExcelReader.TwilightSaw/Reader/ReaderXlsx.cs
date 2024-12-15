using ExcelReader.TwilightSaw.Model;
using OfficeOpenXml;

namespace ExcelReader.TwilightSaw.Reader;

public class ReaderXlsx(string filePath) : IReader
{
    public ReaderItem Read()
    {

        Console.WriteLine("Reading from excel file...");
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
        return new ReaderItem(tables: worksheetList, dbName: Path.GetFileNameWithoutExtension(filePath));
    }

    public void Write(string text)
    {
        throw new NotImplementedException();
    }
}