using System.Text.RegularExpressions;
using System.Xml.Linq;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using OfficeOpenXml;

namespace ExcelReader.TwilightSaw.Service;

public class ReaderService
{
    private string FilePath = @"C:\Users\Alex\source\repos\projects\CodeReviews.Console.ExcelReader\ExcelReader.TwilightSaw\db2.csv";

    public (List<(List<List<string>> table, string tableName)> tables, string dbName) ReadExcel()
    {
            
            Console.WriteLine("Reading from excel file...");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(new FileInfo(FilePath));

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
            return (tables: worksheetList, dbName: Path.GetFileNameWithoutExtension(FilePath));
        
    }

    public (List<(List<List<string>> table, string tableName)> tables, string dbName) ReadCsv()
    {
        var data = File.ReadLines(FilePath).ToList().Select(line => line.Split('\t').ToList()).ToList();
        var tables = new List<(List<List<string>>, string)> { (data, Path.GetFileNameWithoutExtension(FilePath)) };
        return (tables, dbName: Path.GetFileNameWithoutExtension(FilePath));
    }

    public void ReadWord()
    {
        
    }

    public void ReadPdf()
    {
        FilePath =
            @"C:\Users\Alex\source\repos\projects\CodeReviews.Console.ExcelReader\ExcelReader.TwilightSaw\db3.pdf";
        using (var pdfReader = new PdfReader(FilePath))
        using (var pdfDocument = new PdfDocument(pdfReader))
        {
            for (var page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
            {
                var pageText = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page));
            }
        }
    }
}