using ExcelReader.samggannon.Data;
using ExcelReader.samggannon.Models;
using ExcelReader.samggannon.UI;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using System.Configuration;

namespace ExcelReader.samggannon.Services;

internal class ExcelService
{
    private static readonly PlayerContext _playerContext = new();
    private readonly string _filePath;

    public ExcelService()
    {
        _filePath = ConfigurationManager.AppSettings["ExcelSheet"];
    }

    public async Task ReadExcelSheetAsync()
    {
        if (string.IsNullOrWhiteSpace(_filePath) || !File.Exists(_filePath))
        {
            Console.WriteLine($"{_filePath}: did not exist.");
            throw new FileNotFoundException("File not found");
        }

        Console.WriteLine("Reading excel sheet and populating database...");

        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(new FileInfo(_filePath));
            var worksheet = await GetWorkSheetAsync(package);

            if (worksheet == null || worksheet.Dimension == null)
            {
                ConsoleOutput.InformUser("Worksheet is empty or does not exist.");
                return;
            }

            var playerTableData = new List<Player>();
            int firstRow = 1, firstColumn = 1;

            if (!string.IsNullOrWhiteSpace(worksheet.Cells[firstRow, firstColumn].Value?.ToString()))
            {
                for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                {
                    var column = 1;
                    var player = new Player
                    {
                        // Id = worksheet.Cells[row, column++].GetValue<int>(),
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

            ConsoleOutput.ShowTable(playerTableData);
        }
        catch (Exception ex)
        {
            ConsoleOutput.InformUser(ex.Message);
        }
    }

    private async Task<ExcelWorksheet> GetWorkSheetAsync(ExcelPackage package)
    {
        await package.LoadAsync(new FileInfo(_filePath));
        return package.Workbook.Worksheets["PlayerData"];
    }
}
