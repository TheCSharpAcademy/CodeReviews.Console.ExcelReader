namespace ExcelReader;

public class WorkSheet
{
    public int Index { get; set; }
    public string Name { get; set; }
    public List<Dictionary<string, string>> Data { get; set; }
}