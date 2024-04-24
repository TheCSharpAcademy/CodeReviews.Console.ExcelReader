using ExcelReader.Dejmenek.Data;
using ExcelReader.Dejmenek.Data.Interfaces;
using ExcelReader.Dejmenek.Helpers;
using ExcelReader.Dejmenek.Models;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using Spectre.Console;

namespace ExcelReader.Dejmenek.Services;
public class ExcelReaderService : IExcelReaderService
{
    private readonly string _excelFilePath;
    private readonly IItemsRepository _itemsRepository;
    private readonly ISetupDatabase _setupDatabase;

    public ExcelReaderService(IConfiguration configuration, IItemsRepository itemsRepository, ISetupDatabase setupDatabase)
    {
        _excelFilePath = configuration.GetSection("ExcelData").Value;
        _itemsRepository = itemsRepository;
        _setupDatabase = setupDatabase;
    }

    public void Run()
    {
        _setupDatabase.Run();
        var itemsFromExcel = ReadData();
        SendData(itemsFromExcel);
        var itemsFromDatabase = GetItems();
        DataVisualizer.ShowItems(itemsFromDatabase);
    }

    public List<Item> ReadData()
    {
        AnsiConsole.MarkupLine("Reading from excel.");
        List<Item> items = new List<Item>();

        using (var package = new ExcelPackage(_excelFilePath))
        {
            var worksheet = package.Workbook.Worksheets[0];
            int row = 2;
            int column = 1;

            while (string.IsNullOrWhiteSpace(worksheet.Cells[row, column].Value?.ToString()) is false)
            {
                var item = new Item()
                {
                    Id = int.Parse(worksheet.Cells[row, column].Value.ToString()),
                    Name = worksheet.Cells[row, column + 1].Value.ToString(),
                    Quantity = int.Parse(worksheet.Cells[row, column + 2].Value.ToString()),
                    UnitPrice = decimal.Parse(worksheet.Cells[row, column + 3].Value.ToString())
                };

                items.Add(item);

                row++;
            }
        }

        return items;
    }

    public void SendData(List<Item> items)
    {
        AnsiConsole.MarkupLine("Populating the Items table with data from the excel file");
        _itemsRepository.CreateItems(items);
    }

    public List<Item> GetItems()
    {
        AnsiConsole.MarkupLine("Getting data from the Items table");
        return _itemsRepository.GetItems();
    }
}
