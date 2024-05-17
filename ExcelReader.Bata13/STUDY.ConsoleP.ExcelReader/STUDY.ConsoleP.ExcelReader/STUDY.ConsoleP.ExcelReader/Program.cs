using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace STUDY.ConsoleP.ExcelReader;
public class Program
{
    static void Main(string[] args)
    {
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Create or migrate the database
            using (var context = new ExcelDbContext())
            {
                context.Database.Migrate();
            }

            Console.WriteLine("Database migration complete.");
            Console.ReadLine();
           
            // Load ExcelData
            string fileName = "ExcelReaderData.xlsx"; // Replace with the actual file name
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            ExcelDataReader reader = new ExcelDataReader();
            var excelDataList = reader.ReadExcelFile(filePath);

            // Put them in Database
            using (var context = new ExcelDbContext())
            {
                context.ExcelDataModels.AddRange(excelDataList);
                context.SaveChanges();
            }


            Console.WriteLine("Data added to the database.");

            Console.ReadLine();
        }
    }
}
