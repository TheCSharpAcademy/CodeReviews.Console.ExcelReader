using ExcelReader.Models;
using ExcelReader.Services;

namespace ExcelReader.Controllers;
public class ExcelReaderController
{
    private readonly ExcelService _excelService;

    public ExcelReaderController(ExcelService excelService)
    {
        _excelService = excelService;
    }

    public List<DataModel> ReadExcelData()
    {
        return _excelService.ReadExcel();
    }
}