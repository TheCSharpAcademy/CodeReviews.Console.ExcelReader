using OfficeOpenXml;
using System.Data;

namespace edvaudin.ExcelReader
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var file = new FileInfo(@"C:\Users\Ed\Demo.xlsx");
            Console.WriteLine("Loading excel file into DataTable from: " + file.FullName);
            var table = await SpreadsheetLoader.LoadExcelFile(file);

            foreach (DataRow dataRow in table.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Console.WriteLine(item);
                }
            }
        }


        private static void DeleteIfExists(FileInfo file)
        {
            if (file.Exists) { file.Delete(); }
        }
    }
}