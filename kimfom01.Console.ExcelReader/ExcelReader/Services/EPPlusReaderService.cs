using ExcelReader.Models;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace ExcelReader.Services;

public class EPPlusReaderService : IReaderService
{
    private readonly FileInfo _file;

    public EPPlusReaderService(IConfiguration config)
    {
        var filePath = config.GetSection("FilePath").Value;
        var file = new FileInfo(filePath);
        _file = file;
    }

    public async Task<List<Product>> LoadProductsFromExcelAsync()
    {
        var products = new List<Product>();

        using var package = new ExcelPackage(_file);

        var worksheet = await GetWorksheet(package);

        int row = 2, col = 1;

        while (!string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Value?.ToString()))
        {
            var product = CreateProduct(worksheet, row, col);

            ++row;

            products.Add(product);
        }


        return products;
    }

    private Product CreateProduct(ExcelWorksheet worksheet, int row, int col)
    {
        return new Product
        {
            ProductId = GetProductId(worksheet.Cells[row, col].Value.ToString()),
            ProductName = worksheet.Cells[row, col + 1].Value.ToString() ?? "",
            Supplier = worksheet.Cells[row, col + 2].Value.ToString() ?? "",
            ProductCost = GetProductCost(worksheet.Cells[row, col + 3].Value.ToString())
        };
    }

    private async Task<ExcelWorksheet> GetWorksheet(ExcelPackage package)
    {
        await package.LoadAsync(_file);

        var worksheet = package.Workbook.Worksheets["Products"];

        return worksheet;
    }

    private int GetProductId(string? cellData)
    {
        var success = int.TryParse(cellData, out int productId);

        return success ? productId : 0;
    }

    private double GetProductCost(string? cellData)
    {
        var success = double.TryParse(cellData, out double productCost);

        return success ? productCost : 0.0;
    }
}
