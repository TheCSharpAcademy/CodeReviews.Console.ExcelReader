using ExcelReader.Data;
using ExcelReader.Models;
using ExcelReader.Repositories;
using OfficeOpenXml;

namespace ExcelReader.Services;

public class FileProcesserService
{
    private readonly ILogger<FileProcesserService> _logger;
    private readonly IExcelRepository<ExcelData, ExcelContext> _repository;

    public FileProcesserService(ILogger<FileProcesserService> logger, IExcelRepository<ExcelData, ExcelContext> repository)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task ProcessFilesAsync(string path)
    {
        using var p = new ExcelPackage(path);

        var ws = p.Workbook.Worksheets[0];
        int rowCount = ws.Dimension.Rows;
        int colCount = ws.Dimension.Columns;

        var results = new List<Task>();

        for (int row = 2; row <= rowCount; row++)
        {
            results.Add(Task.Run(async () =>
            {
                _logger.LogInformation("Extracting row {row} of {count}", row, rowCount);

                var newRecord = new ExcelData
                {
                    Name = ws.Cells[row, 1].Text,
                    Amount = double.Parse(ws.Cells[row, 2].Text)
                };

                _logger.LogInformation("Committing row {row} of {count} to database", row, rowCount);
                await _repository.CommitEntryAsync(newRecord);
            }));
        }

        await Task.WhenAll(results);
    }
}