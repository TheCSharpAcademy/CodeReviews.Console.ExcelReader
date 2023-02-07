using ExcelReader.Models;

namespace ExcelReader.ConsoleTableViewer;

public interface ITableVisualization
{
    void DisplayProductsTable(List<Product> products);
}
