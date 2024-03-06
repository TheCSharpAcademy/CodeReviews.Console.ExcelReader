namespace ExcelReader.Models;

public class ExcelRowModel
{
    public int RowId {get; set;}
    public List<ExcelRowDataModel<int>> IntRows {get; set;} = [];
    public List<ExcelRowStringModel> StringRows {get; set;} = [];
    public List<ExcelRowDataModel<DateTime>> DateRows {get; set;} = [];
    public List<ExcelRowDataModel<double>> DoubleRows {get; set;} = [];
}