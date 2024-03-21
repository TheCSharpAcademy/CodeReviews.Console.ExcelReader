using Dapper;
using ExcelReader.Data;
using ExcelReader.Models;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Spectre.Console;

namespace ExcelReader.Controller;

public class MainController(DapperContext context, ILogger<MainController> logger)
{
    public void Start()
    {
        logger.Log(LogLevel.Information,"Starting application.");
        
        context.EnsureDeleted();
        context.EnsureCreated();

        SeedData(ReadExcel());
        ShowData(FetchData());
    }

    private void ShowData(IEnumerable<God> gods)
    {
        logger.Log(LogLevel.Information, "Showing the  data from the database.");

        var table = new Table();
        
        var columnNames = typeof(God).GetProperties().Select(p => p.Name).ToList();
        columnNames.ForEach(c => table.AddColumn(c));
        
        gods.ToList().ForEach(g => table.AddRow([g.Name, g.Domain, g.Symbol, g.Fame]));
        
        AnsiConsole.Write(table);
    }

    private IEnumerable<God> FetchData()
    {
        logger.Log(LogLevel.Information, "Fetching data from database.");
        
        using var connection = context.GetConnection();
        return connection.Query<God>("SELECT * FROM Gods");
    }

    private void SeedData(List<God> gods)
    {
        logger.Log(LogLevel.Information, "Seeding data.");

        var connection = context.GetConnection();
        gods.ForEach(g =>
            connection.Execute("INSERT INTO Gods (Name, Domain, Symbol, Fame) VALUES (@Name, @Domain, @Symbol, @Fame)",
                g));
    }

    private List<God> ReadExcel()
    {
        logger.Log(LogLevel.Information,"Reading excel.");

        var gods = new List<God>();
        
        using var package = new ExcelPackage("gods.xlsx");
        var worksheet = package.Workbook.Worksheets[0];
        var rowCount = worksheet.Dimension.Rows;

        for (int row = 2; row <= rowCount; row++)
        {
            gods.Add(new()
            {
                Name =worksheet.Cells[row, 1].Text,
                Domain = worksheet.Cells[row, 2].Text,
                Symbol = worksheet.Cells[row, 3].Text,
                Fame = worksheet.Cells[row, 4].Text,
            });
        }

        logger.Log(LogLevel.Information, "Read {RowCount} rows.", gods.Count);

        return gods;
    }
}