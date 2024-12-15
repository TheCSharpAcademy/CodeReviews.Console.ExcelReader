using ExcelReader.TwilightSaw.Model;
using System.Text.RegularExpressions;

namespace ExcelReader.TwilightSaw.Reader;

public class ReaderCsv(string filePath) : IReader
{
    public ReaderItem Read()
    {
        var data = File.ReadLines(filePath).ToList().Select(line => line.Split(',').Select(value => value.Trim('"', ';')).ToList()).ToList();
        var tableName = Regex.Replace(Path.GetFileNameWithoutExtension(filePath), @"[^a-zA-Z0-9_]", ""); 
        var tables = new List<(List<List<string>>, string)> { (data, tableName) };
        return new ReaderItem(tables, dbName: Path.GetFileNameWithoutExtension(filePath));
    }
}