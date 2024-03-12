namespace ExcelReader.Models;

public class ExcelWorkSheetModel
{
    public int WorkSheetId {get; set;}
    public string? WorkSheetName {get; set;}
    public List<ExcelRowModel> Rows {get; set;} = [];
}