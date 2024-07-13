using ExcelReader.Data;
using ExcelReader.Models;
using OfficeOpenXml;

namespace ExcelReader;

public class Reader
{
    private readonly DataContext context = new();

    public void ReadData()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        Console.WriteLine("Enter the file path");
        string file = Console.ReadLine();
        while (string.IsNullOrEmpty(file) || !File.Exists(file))
        {
            Console.WriteLine("File path can not be empty or the path does not exists");
            file = Console.ReadLine();
        }

        using var package = new ExcelPackage(new FileInfo(file));

        var sheet = package.Workbook.Worksheets[0];

        Console.WriteLine($"Reading data...\n");
        for (var row = 2; row <= sheet.Dimension.End.Row; row++)
        {
            var column = 1;
            context.Add(new EmployeeDetails
            {
                EmployeeId = sheet.Cells[row, column++].GetValue<string>(),
                Name = sheet.Cells[row, column++].GetValue<string>(),
                Department = sheet.Cells[row, column++].GetValue<string>(),
                Position = sheet.Cells[row, column++].GetValue<string>(),
                DateOfBirth = sheet.Cells[row, column++].GetValue<DateTime>(),
                DateOfHire = sheet.Cells[row, column++].GetValue<DateTime>(),
                Email = sheet.Cells[row, column++].GetValue<string>(),
                PhoneNumber = sheet.Cells[row, column++].GetValue<string>(),
                Address = sheet.Cells[row, column++].GetValue<string>(),
                Salary = sheet.Cells[row, column++].GetValue<int>()
            });
        }
        context.SaveChanges();
    }

    public void ShowEmployeeDetails()
    {
        Console.Clear();
        var employees = context.EmployeesDetails.ToList();
        Console.WriteLine("Employees details:");
        foreach (var employee in employees)
        {
            Console.WriteLine($"ID: {employee.EmployeeId,-5} | " +
                              $"Name: {employee.Name,-20} | " +
                              $"Department: {employee.Department,-15} | " +
                              $"Position: {employee.Position,-15} | " +
                              $"DOB: {employee.DateOfBirth?.ToString("yyyy-MM-dd") ?? "N/A",-12} | " +
                              $"Hire Date: {employee.DateOfHire?.ToString("yyyy-MM-dd") ?? "N/A",-12} | " +
                              $"Email: {employee.Email,-25} | " +
                              $"Phone: {employee.PhoneNumber,-12} | " +
                              $"Address: {employee.Address,-30} | " +
                              $"Salary: {employee.Salary,-10}");
        }
    }
}