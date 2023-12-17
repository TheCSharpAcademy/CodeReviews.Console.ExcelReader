using ConsoleTableExt;
using ExcelReader.Speedierone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    order.OrderDate,
                    order.Region,
                    order.RepName,
                    order.Item,
                    order.Units,
                    order.UnitCost,
                    order.TotalCost
                });
            }
            ConsoleTableBuilder.From(tableData).WithColumn("Order Date", "Region", "RepName", "Item", "Units", "Unit Cost", "Total Cost").ExportAndWrite();
        }
    }
}
