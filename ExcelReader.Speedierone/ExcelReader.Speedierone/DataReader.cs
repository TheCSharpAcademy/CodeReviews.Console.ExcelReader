using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using ExcelReader.Speedierone.Model;
using OfficeOpenXml;

namespace ExcelReader.Speedierone
{
    public class DataReader
    {
        public static List<Orders> GetOrders()
        {          
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                List<Orders> order = new List<Orders>();

                string filePath = @"C:\Users\davie\Desktop\SampleExcel.xlsx";

                FileInfo existingFile = new FileInfo(filePath);
            try
            {
                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    int colOrderDate = 1;
                    int colRegion = 2;
                    int colRep = 3;
                    int colItem = 4;
                    int colUnits = 5;
                    int colUnitCost = 6;
                    int colTotal = 7;

                    for (int i = 2; i < worksheet.Dimension.End.Row + 1; i++)
                    {
                        var orderDate = worksheet.Cells[i, colOrderDate].Value;
                        var region = worksheet.Cells[i, colRegion].Value;
                        var repName = worksheet.Cells[i, colRep].Value;
                        var itemName = worksheet.Cells[i, colItem].Value;
                        var units = worksheet.Cells[i, colUnits].Value;
                        var unitCost = worksheet.Cells[i, colUnitCost].Value;
                        var totalCost = worksheet.Cells[i, colTotal].Value;

                        order.Add(new Orders
                        {
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

            return order;
        }
    }
}
