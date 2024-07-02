namespace ExcelReader.Models;

public class Cell
{
    public int CellId { get; set; }
    public int RowId { get; set; }
    public int ColId { get; set; }
    public string Content { get; set; } = "";

    public Row Row { get; set; }
    public Col Col { get; set; }
}