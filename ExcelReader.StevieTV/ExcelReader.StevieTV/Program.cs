using System.Globalization;
using ExcelReader.StevieTV.Database;
using ExcelReader.StevieTV.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using Spectre.Console;

namespace ExcelReader.StevieTV;

internal static class Program
{
    private static readonly EmployeeContext EmployeeContext = new();

    public static void Main()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        InitialiseDatabase();

        ReadData(config["ExcelData"]);

        DisplayDatabase();
    }

    private static void InitialiseDatabase()
    {
        Console.WriteLine("Deleting the database");
        var databaseDeleted = EmployeeContext.Database.EnsureDeleted();

        Console.WriteLine(!databaseDeleted ? "Database did not exist - no need to delete" : "Database successfully deleted");

        Console.WriteLine("Creating the empty database");
        var databaseCreated = EmployeeContext.Database.EnsureCreated();

        if (!databaseCreated)
        {
            Console.WriteLine("Unable to create the database - exiting program");
            return;
        }

        Console.WriteLine("Database successfully created");
    }
    
    private static void ReadData(string? file)
    {
        if (file.IsNullOrEmpty() || !File.Exists(file))
        {
            Console.WriteLine("You must provide a file");
            throw new FileNotFoundException("File not found");
        }

        Console.WriteLine($"Reading the file '{file}' into the database");

        using var package = new ExcelPackage(file);
        var worksheet = package.Workbook.Worksheets[0];
        var rowsAdded = 0;

        for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var column = 1;

            EmployeeContext.Add(new Employee
            {
                EmployeeId = worksheet.Cells[row, column++].GetValue<string>(),
                Name = worksheet.Cells[row, column++].GetValue<string>(),
                JobTitle = worksheet.Cells[row, column++].GetValue<string>(),
                Department = worksheet.Cells[row, column++].GetValue<string>(),
                BusinessUnit = worksheet.Cells[row, column++].GetValue<string>(),
                Gender = worksheet.Cells[row, column++].GetValue<string>(),
                Ethnicity = worksheet.Cells[row, column++].GetValue<string>(),
                Age = worksheet.Cells[row, column++].GetValue<int>(),
                HireDate = DateOnly.FromDateTime(DateTime.FromOADate(worksheet.Cells[row, column++].GetValue<double>())),
                Salary = worksheet.Cells[row, column++].GetValue<double>(),
                BonusPercent = worksheet.Cells[row, column++].GetValue<float>(),
                Country = worksheet.Cells[row, column++].GetValue<string>(),
                City = worksheet.Cells[row, column++].GetValue<string>(),
                ExitDate = worksheet.Cells[row, column].GetValue<double>() == 0 ? default : DateOnly.FromDateTime(DateTime.FromOADate(worksheet.Cells[row, column].GetValue<double>()))
            });

            EmployeeContext.SaveChanges();
            rowsAdded++;
        }

        Console.WriteLine($"{rowsAdded} have been added to the database");
    }

    private static void DisplayDatabase()
    {
        var employees = EmployeeContext.Employees.ToList();
    
        var table = new Table();
        
        foreach (var prop in typeof(Employee).GetProperties())
        {
            table.AddColumn(prop.Name);
        }
        table.Columns[11].RightAligned();
        
        foreach (var employee in employees)
        {
            table.AddRow(employee.Id.ToString(), 
                employee.EmployeeId, 
                employee.Name, 
                employee.JobTitle, 
                employee.Department, 
                employee.BusinessUnit, 
                employee.Gender, 
                employee.Ethnicity, 
                employee.Age.ToString(),
                employee.HireDate.ToShortDateString(),
                employee.Salary.ToString("C0", CultureInfo.CreateSpecificCulture("en-US")), 
                employee.BonusPercent.ToString("0%"), 
                employee.Country, 
                employee.City, 
                employee.ExitDate.ToShortDateString() == "01/01/0001" ? "" : employee.ExitDate.ToShortDateString()
                );
        }
        AnsiConsole.Write(table);
    }

}