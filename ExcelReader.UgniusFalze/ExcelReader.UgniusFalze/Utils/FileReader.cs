using ExcelReader.UgniusFalze.Models;
using OfficeOpenXml;

namespace ExcelReader.UgniusFalze.Services;

public class FileReader : IDisposable
{
    private ExcelPackage Excel { get; set; }
    public FileReader(string file = @"\Data\Planes.xlsx")
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var currentDir = Directory.GetCurrentDirectory();
        Excel = new ExcelPackage(currentDir+file);
    }

    //Not too sure if this is correct way to dispose objects that have a disposable object
    public void Dispose() 
    {
        Excel.Dispose();
        GC.SuppressFinalize(this);
    }

    public List<string>? GetColumns()
    {
        var sheet = Excel.Workbook.Worksheets["Sheet1"];
        if (sheet == null)
        {
            return null;
        }
        var columns = new List<string>();
        for (var i = 1; i <= sheet.Dimension.Columns; i++)
        {
            columns.Add(sheet.Cells[1,i].Text);
        }

        return columns;

    }

    public List<Plane> GetData()
    {
        var sheet = Excel.Workbook.Worksheets["Sheet1"];
        var data = new List<Plane>();
        try
        {
            for (var i = 2; i <= sheet.Dimension.Rows; i++)
            {
                var manufacturer = sheet.Cells[i, 1].Text;
                var model = sheet.Cells[i, 2].Text;
                var type = sheet.Cells[i, 3].Text;
                var maxSpeed = int.Parse(sheet.Cells[i, 4].Text);
                var capacity = int.Parse(sheet.Cells[i, 5].Text);
                var firstFlightDate = DateTime.Parse(sheet.Cells[i, 6].Text);
                var row = new Plane(manufacturer, model, type, maxSpeed, capacity, firstFlightDate);
                data.Add(row);
            }
        }
        catch (Exception ex)
        {
            Display.DisplayIncorrectExcel();
            Console.WriteLine(ex.Message);
        }
        return data;
    }
}