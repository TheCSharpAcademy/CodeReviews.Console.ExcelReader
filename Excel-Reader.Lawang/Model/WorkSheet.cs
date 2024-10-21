namespace Excel_Reader.Lawang.Model;

public class WorkSheet
{
    public string Name { get; set; } = null!;
    public string[] ColumnHeaders { get; set; } = null! ;
    public string[] TableValue { get; set; } = null!;
}
