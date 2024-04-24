using ExcelReader.Dejmenek.Models;

namespace ExcelReader.Dejmenek.Data.Interfaces;
public interface IItemsRepository
{
    List<Item> GetItems();
    void CreateItems(List<Item> items);
}
