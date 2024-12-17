using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ExcelReader.TwilightSaw.Model;

namespace ExcelReader.TwilightSaw.Reader;

public class ReaderDocx(string filePath) : IReader
{
    public ReaderItem Read()
    {
        using var wordDoc = WordprocessingDocument.Open(filePath, false);
        var text = wordDoc.MainDocumentPart.Document.Body;
        var stringBuilder = new StringBuilder();
        foreach (var element in text.Elements())
        {
            if (element is not Paragraph paragraph) continue;
            foreach (var paragraphElement in paragraph.Elements())
            {
                switch (paragraphElement)
                {
                    case Run run:
                    {
                        foreach (var runElement in run.Elements<Text>())
                            stringBuilder.Append(runElement.Text);
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
        var textRows = stringBuilder.ToString().Split("\n").ToList();
        textRows.ForEach(row => row.TrimEnd());

        var cellsList = textRows.Select(r => r.Split(", ").ToList()).ToList();
        var readyCellsList = cellsList.Select(r => r.Select(t => t.TrimEnd()).ToList()).ToList();

        readyCellsList.ForEach(r => r.Remove(""));
        readyCellsList.RemoveAt(cellsList.Count - 1);
        tables.Add((readyCellsList, Path.GetFileNameWithoutExtension(filePath)));

        return new ReaderItem(tables, dbName: Path.GetFileNameWithoutExtension(filePath));
    }
}