using ExcelReader.Models;
using ExcelReader.Utilities;
using OfficeOpenXml;

namespace ExcelReader.Services;
public class ExcelService
{
    internal List<DataModel> ReadExcel()
    {
        var filePath = "/home/nelson/Desktop/C#/AcadamyC#/ExcelReader/ExcelReader.UndercoverDev/ExcelReader/Resources/matches.xlsx";
        var data = new List<DataModel>();


        try
        {
            using var package = new ExcelPackage(new FileInfo(filePath));

            if (package.Workbook.Worksheets.Count > 0)
            {

                var workSheet = package.Workbook.Worksheets[0]; // Use index 0 for the first worksheet
                if (workSheet.Dimension == null || workSheet.Cells.All(cell => cell.Value == null))
                {
                    Logger.Log("[bold][yellow]Excel file has no data.[/][/]");
                    return data;
                }

                var rowCount = workSheet.Dimension?.Rows ?? 0;

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
            }
            else
            {
                Logger.Log("[bold][yellow]No worksheets found in the Excel file.[/][/]");
                return data;
            }
            return data;
        }
        catch (Exception ex)
        {
            Logger.Log($"[red]Error reading Excel file: {ex.Message}[/]");
            return data;
        }
    }
}
