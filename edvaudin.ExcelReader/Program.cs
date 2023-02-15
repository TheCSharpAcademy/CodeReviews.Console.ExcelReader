using OfficeOpenXml;

namespace edvaudin.ExcelReader;

internal static class Program
{
    private static readonly DbFactory dbFactory = new();
    static async Task Main()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var file = new FileInfo("Demo.xlsx");

        Console.WriteLine("Loading excel file into DataTable from: " + file.FullName);
        var table = await SpreadsheetLoader.LoadExcelFile(file);

        Console.WriteLine("Deleting pre-existing database...");
        dbFactory.DropExistingDataBase();

        Console.WriteLine("Creating new database...");
        dbFactory.CreateNewDatabase();

        Console.WriteLine("Transferring DataTable structure to database...");
        dbFactory.CreateTable(table);

        Console.WriteLine("Transferring DataTable contents to database...");
        dbFactory.InsertIntoTable(table);

        Console.WriteLine("Data transfer from Excel to SQL Server complete\n");
        dbFactory.DisplayTable();
    }
}