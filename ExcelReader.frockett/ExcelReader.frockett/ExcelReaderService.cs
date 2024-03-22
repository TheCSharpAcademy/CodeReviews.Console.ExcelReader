using ExcelReader.frockett.Models;
using OfficeOpenXml;

namespace ExcelReader.frockett;

internal static class ExcelReaderService
{
    public static List<Employee> LoadEmployees(string filePath = "Data\\EmployeeData.xlsx")
    {
        List<Employee> employees = new();

        using var package = new ExcelPackage(new FileInfo (filePath));

        ExcelWorksheet sheet = package.Workbook.Worksheets[0];

        // Rows are counted dynamically, more employees can be added without modifying the program
        int totalrows = sheet.Dimension.Rows;

        for (int row = 2; row < totalrows + 1; row++)
        {
            employees.Add(new Employee
            {
                EmployeeId = sheet.Cells[row, 1].Text,
                EmployeeName = sheet.Cells[row, 2].Text,
                JobTitle = sheet.Cells[row, 3].Text,
                Department = sheet.Cells[row, 4].Text,
                BusinessUnit = sheet.Cells[row, 5].Text,
                Gender = sheet.Cells[row, 6].Text,
                Age = int.Parse(sheet.Cells[row, 7].Text),
                HireDate = DateOnly.Parse(sheet.Cells[row, 8].Text),
                AnnualSalary = int.Parse(sheet.Cells[row, 9].Text, System.Globalization.NumberStyles.Currency),
                BonusPercent = int.Parse(sheet.Cells[row, 10].Text.Replace("%","")),
                Country = sheet.Cells[row, 11].Text,
                City = sheet.Cells[row, 12].Text,
                ExitDate = string.IsNullOrEmpty(sheet.Cells[row, 13].Text) ? null : DateOnly.Parse(sheet.Cells[row, 13].Text)
            });

        }
        return employees;
    }
}
