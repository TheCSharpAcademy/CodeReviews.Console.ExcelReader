using System.Globalization;
using ExcelReader.kalsson.Models;
using OfficeOpenXml;

namespace ExcelReader.kalsson.Services;

public class ExcelService
{
    public List<EmployeeModel> ReadDataFromExcel(string filePath)
        {
            var employees = new List<EmployeeModel>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filePath} does not exist.");
            }

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // Check if there are any worksheets in the workbook
                if (package.Workbook.Worksheets.Count == 0)
                {
                    throw new InvalidOperationException("The workbook does not contain any worksheets.");
                }

                var worksheet = package.Workbook.Worksheets.First();

                // Check if the worksheet has any rows
                if (worksheet.Dimension == null)
                {
                    throw new InvalidOperationException("The worksheet does not contain any data.");
                }

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                Console.WriteLine($"Worksheet contains {rowCount} rows and {colCount} columns.");

                // Read data from the worksheet
                for (int row = 2; row <= rowCount; row++)
                {
                    var eeid = worksheet.Cells[row, 1].Text;
                    var fullName = worksheet.Cells[row, 2].Text;
                    var jobTitle = worksheet.Cells[row, 3].Text;
                    var department = worksheet.Cells[row, 4].Text;
                    var businessUnit = worksheet.Cells[row, 5].Text;
                    var gender = worksheet.Cells[row, 6].Text;
                    var ethnicity = worksheet.Cells[row, 7].Text;
                    var ageText = worksheet.Cells[row, 8].Text;
                    var hireDateText = worksheet.Cells[row, 9].Text;
                    var annualSalaryText = worksheet.Cells[row, 10].Text;
                    var bonusPercentageText = worksheet.Cells[row, 11].Text;
                    var country = worksheet.Cells[row, 12].Text;
                    var city = worksheet.Cells[row, 13].Text;
                    var exitDateText = worksheet.Cells[row, 14].Text;

                    Console.WriteLine($"Row {row}: EEID = {eeid}, FullName = {fullName}, JobTitle = {jobTitle}, Department = {department}, BusinessUnit = {businessUnit}, Gender = {gender}, Ethnicity = {ethnicity}, Age = {ageText}, HireDate = {hireDateText}, AnnualSalary = {annualSalaryText}, BonusPercentage = {bonusPercentageText}, Country = {country}, City = {city}, ExitDate = {exitDateText}");

                    if (!string.IsNullOrWhiteSpace(eeid) && 
                        int.TryParse(ageText, out int age) &&
                        DateTime.TryParse(hireDateText, out DateTime hireDate))
                    {
                        // Parse AnnualSalary and BonusPercentage by stripping non-numeric characters
                        decimal annualSalary = decimal.Parse(new string(annualSalaryText.Where(char.IsDigit).ToArray()), CultureInfo.InvariantCulture);
                        decimal bonusPercentage = decimal.Parse(new string(bonusPercentageText.Where(char.IsDigit).ToArray()), CultureInfo.InvariantCulture);

                        DateTime? exitDate = null;
                        if (DateTime.TryParse(exitDateText, out DateTime exitDateValue))
                        {
                            exitDate = exitDateValue;
                        }

                        var employee = new EmployeeModel
                        {
                            EEID = eeid,
                            FullName = fullName,
                            JobTitle = jobTitle,
                            Department = department,
                            BusinessUnit = businessUnit,
                            Gender = gender,
                            Ethnicity = ethnicity,
                            Age = age,
                            HireDate = hireDate,
                            AnnualSalary = annualSalary,
                            BonusPercentage = bonusPercentage,
                            Country = country,
                            City = city,
                            ExitDate = exitDate
                        };

                        employees.Add(employee);
                        Console.WriteLine($"Added Employee: {employee.EEID}, {employee.FullName}");
                    }
                }
            }

            return employees;
        }
}