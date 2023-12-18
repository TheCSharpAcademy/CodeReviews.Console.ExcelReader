using ExcelReader.Speedierone.Model;
using OfficeOpenXml;

namespace ExcelReader.Speedierone
{
    public class DataReader
    {
        public static List<Orders> GetOrders()
        {
                Console.WriteLine("Reading excel table...");
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                List<Orders> order = new List<Orders>();

                string filePath = @"C:\Users\davie\Desktop\SampleExcel.xlsx";

                FileInfo existingFile = new FileInfo(filePath);
            try
            {
                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    int colOrderId = 1;
                    int colOrderDate = 2;
                    int colRegion = 3;
                    int colRep = 4;
                    int colItem = 5;
                    int colUnits = 6;
                    int colUnitCost = 7;
                    int colTotal = 8;

                    for (int i = 2; i < worksheet.Dimension.End.Row + 1; i++)
                    {
                        var orderId = worksheet.Cells[i, colOrderId].Value;
                        var orderDate = worksheet.Cells[i, colOrderDate].Value;
                        var region = worksheet.Cells[i, colRegion].Value;
                        var repName = worksheet.Cells[i, colRep].Value;
                        var itemName = worksheet.Cells[i, colItem].Value;
                        var units = worksheet.Cells[i, colUnits].Value;
                        var unitCost = worksheet.Cells[i, colUnitCost].Value;
                        var totalCost = worksheet.Cells[i, colTotal].Value;

                        order.Add(new Orders
                        {
                            Id = Convert.ToInt32(orderId),
                            OrderDate = orderDate.ToString(),
                            Region = region.ToString(),
                            RepName = repName.ToString(),
                            Item = itemName.ToString(),
                            Units = Convert.ToInt32(units),
                            UnitCost = Convert.ToDouble(unitCost),
                            TotalCost = Convert.ToDouble(totalCost)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Checking and deleting existing data....");
            DataInsert.DeleteExistingRecords();
            Console.WriteLine("Creating SqlServer database....");
            DataInsert.InsertIntoSqlServer(order);
            return order;
        }
    }
}
