namespace ExcelReader.Models;

public class ExcelCellString
{
    public string? CellTitle {get; set;}
    public string? CellValue {get; set;}

    public override string ToString()
    {
        return $"{CellTitle}: {CellValue}";
    }
}