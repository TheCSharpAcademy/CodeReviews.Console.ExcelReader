using ExcelReader.Dejmenek.Models;

namespace ExcelReader.Dejmenek.Services;
public interface IExcelReaderService
{
    void Run();
    void SendData(List<Item> items);
    List<Item> GetItems();
    List<Item> ReadData();
}
