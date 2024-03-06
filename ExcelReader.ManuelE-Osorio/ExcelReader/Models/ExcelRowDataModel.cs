namespace ExcelReader.Models;

public class ExcelRowDataModel<T> where T : struct
{
    
    // public int RowId {get; set;}
    public string? RowTitle {get; set;}
    public T? RowValue {get; set;}
}