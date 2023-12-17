using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader.Speedierone.Model
{
    public class Orders
    {
        public string OrderDate { get; set; }
        public string Region { get; set; }
        public string RepName { get; set; }
        public string Item {  get; set; }
        public int Units { get; set; }
        public double UnitCost { get; set; }
        public double TotalCost { get; set; }
    }
}
