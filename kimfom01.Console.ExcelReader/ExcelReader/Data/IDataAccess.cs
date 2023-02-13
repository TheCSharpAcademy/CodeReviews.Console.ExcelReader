using ExcelReader.Models;

namespace ExcelReader.Data;

public interface IDataAccess
{
    Task<List<Product>> GetProductsAsync();
    Task AddProducts(List<Product> products);
}
