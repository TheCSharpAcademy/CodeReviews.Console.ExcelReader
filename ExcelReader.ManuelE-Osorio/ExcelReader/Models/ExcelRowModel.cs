namespace ExcelReader.Models;

public class ExcelRowModel
{
    public int RowId {get; set;}
    public List<ExcelCellData<int>> IntCells {get; set;} = [];
    public List<ExcelCellString> StringCells {get; set;} = [];
    public List<ExcelCellData<DateTime>> DateCells {get; set;} = [];
    public List<ExcelCellData<double>> DoubleCells {get; set;} = [];
}