namespace ExcelReader.Models;

public class ExcelRowDataModel<T> where T : struct
{    
    public string? RowTitle {get; set;}
    public T? RowValue {get; set;}
}