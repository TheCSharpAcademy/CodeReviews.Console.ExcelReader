namespace ExcelReader.Speedierone.Model
{
    public class Orders
    {
        public int Id {  get; set; } 
        public string OrderDate { get; set; }
        public string Region { get; set; }
        public string RepName { get; set; }
        public string Item {  get; set; }
        public int Units { get; set; }
        public double UnitCost { get; set; }
        public double TotalCost { get; set; }
    }
}
