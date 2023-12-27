using ConsoleTableExt;
using ExcelReader.Speedierone.Model;

namespace ExcelReader.Speedierone
{
    public class TableLayout
    {
        public static void DisplayTable(List<Orders> orders)
        {
            var tableData = new List<List<Object>>();
            foreach (Orders order in orders)
            {
                tableData.Add(new List<object>
                {
                    order.Id,
                    order.OrderDate,
                    order.Region,
                    order.RepName,
                    order.Item,
                    order.Units,
                    order.UnitCost,
                    order.TotalCost
                });
            }
            ConsoleTableBuilder.From(tableData).WithColumn("Id", "Order Date", "Region", "RepName", "Item", "Units", "Unit Cost", "Total Cost").ExportAndWrite();
        }

        public static void ReadSqlServer()
        {
            Console.Clear();
            using var db = new OrdersContext();

            var orders = db.Orders
                .OrderBy(x => x.OrderDate)
                .ToList();

            foreach (var order in orders)
            {
                orders.Add(new Orders
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    Region = order.Region,
                    RepName = order.RepName,
                    Item = order.Item,
                    Units = order.Units,
                    UnitCost = order.TotalCost,
                    TotalCost = order.TotalCost
                });
            }
            DisplayTable(orders);
        }
    }
}
