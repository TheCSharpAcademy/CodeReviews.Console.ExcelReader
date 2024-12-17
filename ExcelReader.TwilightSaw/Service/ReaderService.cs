using System.Text.RegularExpressions;
using ExcelReader.TwilightSaw.Model;
using ExcelReader.TwilightSaw.Reader;

namespace ExcelReader.TwilightSaw.Service;

public class ReaderService(string filePath)
{
    public ReaderItem ReadFormat()
    {
        filePath = filePath.Replace(" ", "");
        var file = Path.GetExtension(filePath).ToLower() switch
        {
            ".pdf" => new ReaderPdf(filePath).Read(),
            ".csv" => new ReaderCsv(filePath).Read(),
            ".xlsx" => new ReaderXlsx(filePath).Read(),
            ".docx" => new ReaderDocx(filePath).Read(),
            _ => default
        };
        if (file == null) return null;
        file.DbName = Regex.Replace(file.DbName, @"[^a-zA-Z0-9_]", "");
        return file;
    }

    public void Write()
    {
        new ReaderXlsx(filePath).Write();
    }
}

