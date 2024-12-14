using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

using OfficeOpenXml;

namespace ExcelReader.TwilightSaw.Service;

public class ReaderService(string filePath)
{
    public (List<(List<List<string>> table, string tableName)> tables, string dbName) ChooseFormat()
    {
        return Path.GetExtension(filePath).ToLower() switch
        {
            //not reliable method
            ".pdf" => ReadPdf(),
            ".csv" => ReadCsv(),
            ".docx" => ReadWord(),
            ".xlsx" => ReadExcel(),
            _ => default
        };
    }
    public (List<(List<List<string>> table, string tableName)> tables, string dbName) ReadExcel()
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
                        rowValues.Add(worksheet.Cells[i, j].Value.ToString());
                    worksheetRows.Add(rowValues);
                }
                worksheetList.Add((worksheetRows, worksheet.Name));
            }
            return (tables: worksheetList, dbName: Path.GetFileNameWithoutExtension(filePath));
    }

    public (List<(List<List<string>> table, string tableName)> tables, string dbName) ReadCsv()
    {
        var data = File.ReadLines(filePath).ToList().Select(line => line.Split('\t').ToList()).ToList();
        var tables = new List<(List<List<string>>, string)> { (data, Path.GetFileNameWithoutExtension(filePath)) };
        return (tables, dbName: Path.GetFileNameWithoutExtension(filePath));
    }

    public (List<(List<List<string>> table, string tableName)> tables, string dbName) ReadWord()
    {
        using var wordDoc = WordprocessingDocument.Open(filePath, false);
        var text = wordDoc.MainDocumentPart.Document.Body;

        var stringBuilder = new StringBuilder();
        foreach (var t in text.Elements())
        {
            if (t is not Paragraph paragraph) continue;
            foreach (var p in paragraph.Elements())
            {
                switch (p)
                {
                    case Run run:
                    {
                        foreach (var r in run.Elements<Text>())
                            stringBuilder.Append(r.Text);
                        break;
                    }
                    case Break:
                        stringBuilder.AppendLine();
                        break;
                }
            }

            stringBuilder.AppendLine();
        }

        var tables = new List<(List<List<string>>, string)>();

        var pageText = stringBuilder.ToString().Split("\n").ToList();
        pageText.ForEach(r => r.TrimEnd());
        var y = pageText.Select(r => r.Split(", ").ToList()).ToList();
        var y1 = y.Select(r => r.Select(t => t.TrimEnd()).ToList()).ToList();
        y1.ForEach(r => r.Remove(""));
        y1.RemoveAt(y.Count-1); //from where?
        tables.Add((y1, Path.GetFileNameWithoutExtension(filePath)));

        return (tables, dbName: Path.GetFileNameWithoutExtension(filePath));
    }

    public (List<(List<List<string>> table, string tableName)> tables, string dbName) ReadPdf()
    {
        using var pdfReader = new PdfReader(filePath);
        using var pdfDocument = new PdfDocument(pdfReader);
        var tables = new List<(List<List<string>>, string)>();
        for (var page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
        {
            var pageText = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page)).Split("\n").ToList();
            pageText.ForEach(r => r.TrimEnd());
            var y = pageText.Select(r => r.Split(", ").ToList()).ToList();
            y.ForEach(r => r.Remove(""));
            tables.Add((y, Path.GetFileNameWithoutExtension(filePath))); 
        }
        return (tables, dbName: Path.GetFileNameWithoutExtension(filePath));
    }
}