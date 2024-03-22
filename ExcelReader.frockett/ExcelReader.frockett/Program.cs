using ExcelReader.frockett;
using ExcelReader.frockett.Database;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var context = new EmployeeContext();

using (context)
{
    try
    {
        string deleteMessage = context.Database.EnsureDeleted() ? "Database Deleted!" : "No database found! Continuing to create...";

        Console.WriteLine(deleteMessage);

        if (context.Database.EnsureCreated())
        {
            Console.WriteLine("New database created!");
        }
        else
        {
            Console.WriteLine("An error occurred creating the new database. Terminating program. Press enter to accept...");
            Console.ReadLine();
            Environment.Exit(1);
        }

        // There is an optional parameter for the file path in case you place it in a different folder
        // Data is a modified version of the free employee data sheet found here: https://www.thespreadsheetguru.com/sample-data/
        var employees = ExcelReaderService.LoadEmployees();
        Console.WriteLine("Reading employees from spreadsheet...");
        Thread.Sleep(1000);

        context.AddRange(employees);
        Console.WriteLine("Writing employees to database...");
        context.SaveChanges();
        Thread.Sleep(1000);

        Console.Clear();
        Console.WriteLine("Preparing display table. You are advised to maximize your console window. Press enter when ready...");
        Console.ReadLine();
        Console.Clear();

        var employeesToPrint = context.Employees.ToList();

        Display.PrintEmployees(employeesToPrint);

    }
    catch (Exception ex)
    {
        Console.WriteLine($"An exception occurred: {ex.Message}");
    }
}


