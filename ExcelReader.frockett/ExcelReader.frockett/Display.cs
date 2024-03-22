using Spectre.Console;
using ExcelReader.frockett.Models;

namespace ExcelReader.frockett;

internal static class Display
{
    public static void PrintEmployees(List<Employee> employees)
    {
        Spectre.Console.Table table = new();

        table.Title("Employees");
        table.AddColumns(new[] { "ID", "Name", "Title", "Dept", "Business Unit", "Gender", "Age", "Hire Date", "Annual Salary", "Bonus Percent", "Country", "City", "Exit Date" });

        foreach (Employee employee in employees)
        {
            table.AddRow(
                employee.EmployeeId,
                employee.EmployeeName,
                employee.JobTitle,
                employee.Department,
                employee.BusinessUnit,
                employee.Gender,
                employee.Age.ToString(),
                employee.HireDate.ToString("yyyy-MM-dd"),
                $"{employee.AnnualSalary:C}",
                $"{employee.BonusPercent}%",
                employee.Country,
                employee.City,
                employee.ExitDate?.ToString("yyyy-MM-dd") ?? "N/A");
        }

        AnsiConsole.Write(table);

        Console.WriteLine("Press enter to exit program...");
        Console.ReadLine();
    }
}
