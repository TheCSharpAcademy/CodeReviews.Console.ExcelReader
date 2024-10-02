using ExcelReader;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Configuration;
using ExcelReader.Models;

internal class Program
{
    private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

    private static void Main(string[] args)
    {
        List<Footballer> footballers = [];

        Console.WriteLine("Establishing a connection to the database...\n");
        var options = new DbContextOptionsBuilder<FootballersContext>()
            .UseSqlServer(_connectionString)
            .Options;
        FootballersContext context = new FootballersContext(options);

        Console.WriteLine("Creating table...\n");
        context.Database.EnsureDeleted();
        context.Database.Migrate();

        string filePath = "C:\\Users\\mikolajewskj\\source\\repos\\CodeReviews.Console.ExcelReader\\ExcelReader.jakubmikolajewski\\ExcelReader\\Footballers.xlsx";
        FileInfo fileInfo = new FileInfo(filePath);

        Console.WriteLine("Reading data from the excel file...\n");
        using (ExcelPackage package = new(fileInfo))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            footballers = worksheet.Cells["A1:E5"].ToCollection<Footballer>();
        }

        Console.WriteLine("Seeding data to the database...\n");
        context.Set<Footballer>().AddRange(footballers);
        context.SaveChanges();

        Console.WriteLine("Fetching data from the database...\n");
        var result = context.Set<Footballer>().ToList();

        Console.WriteLine("Displaying results:\n");
        result.ForEach(footballer => Console.WriteLine($"{footballer.Name}, {footballer.Position}, {footballer.Nationality}, {footballer.Club}, {footballer.Age}"));
    }

}