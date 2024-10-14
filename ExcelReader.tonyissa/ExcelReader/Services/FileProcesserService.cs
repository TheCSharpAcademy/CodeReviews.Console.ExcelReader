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
            var currentRow = row;

            _logger.LogInformation("Extracting row {row} of {count}", currentRow, rowCount);

            var newRecord = new ExcelData
            {
                Name = ws.Cells[currentRow, 1].Text,
                Amount = decimal.Parse(ws.Cells[currentRow, 2].Text)
            };

            _logger.LogInformation("Committing row {row} of {count} to database", currentRow, rowCount);
            await _repository.CommitEntryAsync(newRecord);
        }
    }
}