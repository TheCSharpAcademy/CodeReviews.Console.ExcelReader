using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using STUDY.ConsoleP.ExcelReaderTryTwo;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

Console.WriteLine("Press Any Key To Create Database");
Console.ReadLine();

CreateDatabase();

Console.WriteLine("Database Created");

Console.WriteLine("Press Any Key To Load Excel Data");
Console.ReadLine();

var excelData = LoadExcelData();

Console.WriteLine("Excel Data Loaded");

Console.WriteLine("Press Any Key To Update Database");
Console.ReadLine();

UpdateDatabase(excelData);

Console.WriteLine("Database Updated");

Console.WriteLine("Press Any Ket To Exit");
Console.ReadLine();
void CreateDatabase()
{
    using (var context = new ExcelDbContext())
    {
        if (context.Database.CanConnect())
        {
            context.Database.EnsureDeleted();
            Console.WriteLine("Existing database deleted.");
        }

        context.Database.Migrate();
        Console.WriteLine("Database migration complete.");
    }
}
List<ExcelDbModel> LoadExcelData()
{
    string fileName = "ExcelReaderData.xlsx";
    string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
    ExcelDataReader reader = new ExcelDataReader();

    return reader.ReadExcelFile(filePath);
}
void UpdateDatabase(List<ExcelDbModel> excelData)
{
    using (var context = new ExcelDbContext())
    {
        context.ExcelDataModels.AddRange(excelData);
        context.SaveChanges();
    }
}
