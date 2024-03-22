using ExcelReader.frockett;
using ExcelReader.frockett.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;

var builder = Host.CreateApplicationBuilder();

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


