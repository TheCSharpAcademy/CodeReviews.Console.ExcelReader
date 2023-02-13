using ConsoleTableExt;
using ExcelReader.Models;

namespace ExcelReader.ConsoleTableViewer;

public class ConsoleTableBuilderVisualization : ITableVisualization
{
    public void DisplayProductsTable(List<Product> products)
    {
        ConsoleTableBuilder
            .From(products)
            .WithTitle("Products")
            .ExportAndWriteLine();
    }
}
