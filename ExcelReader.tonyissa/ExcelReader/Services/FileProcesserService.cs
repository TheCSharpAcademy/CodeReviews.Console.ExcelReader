using ExcelReader.Data;
using ExcelReader.Models;
using ExcelReader.Repositories;
using OfficeOpenXml;
using Spectre.Console;

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

        for (int row = 2; row <= rowCount; row++)
        {
            var currentRow = row;

            _logger.LogInformation("Extracting row {row} of {count}", currentRow - 1, rowCount - 1);

            var newRecord = new ExcelData
            {
                Name = ws.Cells[currentRow, 1].Text,
                Amount = decimal.Parse(ws.Cells[currentRow, 2].Text)
            };

            _logger.LogInformation("Committing row {row} of {count} to database", currentRow - 1, rowCount - 1);
            await _repository.CommitEntryAsync(newRecord);
        }
    }

    public async Task DisplayListAfterFinished()
    {
        var list = await _repository.RetrieveEntriesAsync();

        var table = new Table { Title = new TableTitle("Entries") };
        table.AddColumns("Id", "Name", "Amount");

        foreach (var item in list)
        {
            table.AddRow(item.Id.ToString(), item.Name, item.Amount.ToString());
        }

        AnsiConsole.Write(table);
    }
}