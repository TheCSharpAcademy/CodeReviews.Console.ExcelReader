using System.Text.RegularExpressions;
using OfficeOpenXml;

namespace ExcelReader.TwilightSaw.Controller;

public class ReaderController
{
    private readonly string _filePath = @"C:\Users\Alex\source\repos\projects\CodeReviews.Console.ExcelReader\ExcelReader.TwilightSaw\db.xlsx";

    public (List<(List<List<string>> table, string tableName)> tables, string dbName) Read()
    {
        
            Console.WriteLine("Reading from excel file...");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(new FileInfo(_filePath));

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
                        rowValues.Add(worksheet.Cells[i, j].Value.ToString());
                    worksheetRows.Add(rowValues);
                }
                worksheetList.Add((worksheetRows, worksheet.Name));
            }

            var regex = new Regex(@"\.[a-z]");
            return (tables: worksheetList, dbName: Path.GetFileNameWithoutExtension(_filePath));
        
    }
}