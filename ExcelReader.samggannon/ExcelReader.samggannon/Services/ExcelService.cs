using ExcelReader.samggannon.Data;
using ExcelReader.samggannon.Models;
using ExcelReader.samggannon.UI;
using OfficeOpenXml;
using System.Configuration;

namespace ExcelReader.samggannon.Services;

internal class ExcelService
{
    private static readonly PlayerContext _playerContext = new();
    private readonly string _filePath;

    public ExcelService()
    {
        // dynamically retrieve file path from app.config file
        // _filePath = ConfigurationManager.AppSettings["ExcelSheet"];

        // retrieve file path from the directory of the project.
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string folderPath = Path.Combine(baseDirectory, "Data");
        _filePath = Path.Combine(folderPath, "SampleFootballPlayerData.xlsx");
    }

    public async Task ReadExcelSheetAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_filePath) || !File.Exists(_filePath))
            {
                Console.WriteLine($"{_filePath}: did not exist.");
                throw new FileNotFoundException("File not found"); 
            }

            Console.WriteLine("Reading excel sheet and populating database...");

        
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(new FileInfo(_filePath));
            var worksheet = await GetWorkSheetAsync(package);

            if (worksheet == null || worksheet.Dimension == null)
            {
                Console.WriteLine("Worksheet is empty or does not exist.");
                return;
            }

            var playerTableData = new List<Player>();
            int firstRow = 1, firstColumn = 1;

            if (!string.IsNullOrWhiteSpace(worksheet.Cells[firstRow, firstColumn].Value?.ToString()))
            {
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var column = 1;
                    var player = new Player
                    {
                        Year = worksheet.Cells[row, column++].GetValue<string>(),
                        Team = worksheet.Cells[row, column++].GetValue<string>(),
                        Name = worksheet.Cells[row, column++].GetValue<string>(),
                        Number = worksheet.Cells[row, column++].GetValue<string>(),
                        Pos = worksheet.Cells[row, column++].GetValue<string>(),
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
                Console.WriteLine("Ensure data is present in the excel sheet. Make sure cell A:A has input");
            }

            ConsoleOutput.ShowTable(playerTableData);
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }

    private async Task<ExcelWorksheet> GetWorkSheetAsync(ExcelPackage package)
    {
        await package.LoadAsync(new FileInfo(_filePath));
        return package.Workbook.Worksheets["PlayerData"];
    }
}
