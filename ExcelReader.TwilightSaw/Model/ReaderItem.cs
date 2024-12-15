namespace ExcelReader.TwilightSaw.Model;

public class ReaderItem(List<(List<List<string>> table, string tableName)> tables, string dbName)
{
    public List<(List<List<string>> table, string tableName)> Tables { get; set; } = tables;
    public string DbName { get; set; } = dbName;
}