using ExcelReader.Speedierone.Model;

namespace ExcelReader.Speedierone;

class Program
{
    static void Main(string[] args)
    {
        List<Orders> orders = DataReader.GetOrders();
        TableLayout.DisplayTable(orders);
        Console.ReadLine();
    }
}
