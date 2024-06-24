using ExcelReader.samggannon.Data;
using ExcelReader.samggannon.Models;
using ExcelReader.samggannon.UI;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;

namespace ExcelReader.samggannon.Services;

internal class ExcelService
{
    private static readonly PlayerContext _playerContext = new();
    private readonly FileInfo _fileName;

    public ExcelService()
    {

    }

    public async void ReadExcelSheet(string fileName)
    {
        ExcelWorksheet? worksheet = null;
        List<Player> playerTableData = new List<Player>();

        if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
        {
            Console.WriteLine($"{fileName}: did not exist.");
            throw new FileNotFoundException("File not found");
        }

        Console.WriteLine("Reading excel sheet and populating database...");

        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(_fileName);
            worksheet = await GetWorkSheet(package);

            int firstRow = 1;  int firstColumn = 1;

            if (!string.IsNullOrWhiteSpace(worksheet.Cells[firstRow, firstColumn].Value?.ToString()))
            {
                for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                {
                    var column = 1;

                    var player = new Player
                    {
                        Id = worksheet.Cells[row, column++].GetValue<int>(),
                        Year = worksheet.Cells[row, column++].GetValue<string>(),
                        Team = worksheet.Cells[row, column++].GetValue<string>(),
                        Name = worksheet.Cells[row, column++].GetValue<string>(),
                        Number = worksheet.Cells[row, column++].GetValue<string>(),
                        Position = worksheet.Cells[row, column++].GetValue<string>(),
                        Height = worksheet.Cells[row, column++].GetValue<string>(),
                        Weight = worksheet.Cells[row, column++].GetValue<string>(),
                        Age = worksheet.Cells[row, column++].GetValue<string>(),
                        Experience = worksheet.Cells[row, column++].GetValue<string>(),
                        College = worksheet.Cells[row, column++].GetValue<string>()
                    };

                    _playerContext.Add(player);
                    playerTableData.Add(player);
                }

                await _playerContext.SaveChangesAsync();
            }
            else
            {
                ConsoleOutput.InformUser("Ensure data is present in the excel sheet. Make sure cell A:A has input");
            }
        } 
        catch(Exception ex) 
        {
            ConsoleOutput.InformUser(ex.Message);
        }

        ConsoleOutput.ShowTable(playerTableData);
    }
     
    private async Task<ExcelWorksheet> GetWorkSheet(ExcelPackage package)
    {
        await package.LoadAsync(_fileName);

        var worksheet = package.Workbook.Worksheets["Sheet1"];

        return worksheet;
    }
}
