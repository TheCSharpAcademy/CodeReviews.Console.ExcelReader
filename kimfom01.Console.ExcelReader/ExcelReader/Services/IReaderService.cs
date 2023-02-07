using ExcelReader.Models;

namespace ExcelReader.Services;

public interface IReaderService
{
    public Task<List<Product>> LoadProductsFromExcelAsync();
}
