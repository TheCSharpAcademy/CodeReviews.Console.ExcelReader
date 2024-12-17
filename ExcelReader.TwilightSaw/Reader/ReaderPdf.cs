using DocumentFormat.OpenXml.Packaging;
using ExcelReader.TwilightSaw.Model;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;

namespace ExcelReader.TwilightSaw.Reader;

public class ReaderPdf(string filePath) : IReader
{
    public ReaderItem Read()
    {
        using var pdfReader = new PdfReader(filePath);
        using var pdfDocument = new PdfDocument(pdfReader);

        var tables = new List<(List<List<string>>, string)>();
        for (var page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
        {
            var textRows = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page)).Split("\n").ToList();
            textRows.ForEach(row => row.TrimEnd());
            var cellsList = textRows.Select(row => row.Split(", ").ToList()).ToList();
            cellsList.ForEach(cell => cell.Remove(""));
            tables.Add((cellsList, Path.GetFileNameWithoutExtension(filePath)));
        }
        return new ReaderItem(tables, dbName: Path.GetFileNameWithoutExtension(filePath));
    }
}