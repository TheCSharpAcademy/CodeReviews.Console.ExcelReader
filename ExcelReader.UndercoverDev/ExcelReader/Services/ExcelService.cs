using ExcelReader.Models;
using OfficeOpenXml;

namespace ExcelReader.Services;
public class ExcelService
{
    internal List<DataModel> ReadExcel()
    {
        var filePath = "../Resources/matches.xlsx";
        var data = new List<DataModel>();

        // Read data from Excel file and populate the data list
        using var package = new ExcelPackage(new FileInfo(filePath));

        var workSheet = package.Workbook.Worksheets[0];
        var rowCount = workSheet.Dimension.Rows;

        for (var row = 2; row <= rowCount; row++)
        {
            var dataModel = new DataModel
            {
                Date = workSheet.Cells[row, 1].Value?.ToString() ?? string.Empty,
                League = workSheet.Cells[row, 2].Value?.ToString() ?? string.Empty,
                Home = workSheet.Cells[row, 3].Value?.ToString() ?? string.Empty,
                Away = workSheet.Cells[row, 4].Value?.ToString() ?? string.Empty,
                HomeProbability = workSheet.Cells[row, 5].Value?.ToString() ?? string.Empty,
                AwayProbability = workSheet.Cells[row, 6].Value?.ToString() ?? string.Empty,
                OverTwoGoals = workSheet.Cells[row, 7].Value?.ToString() ?? string.Empty
            };
            data.Add(dataModel);
        }
        return data;
    }
}