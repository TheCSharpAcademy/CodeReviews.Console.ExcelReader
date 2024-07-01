using ExcelReader.kalsson.Data;
using ExcelReader.kalsson.Models;
using ExcelReader.kalsson.Services;
using Spectre.Console;

try
{
    // Print the current directory to verify the working directory
    Console.WriteLine("Current Directory: " + Directory.GetCurrentDirectory());

    string filePath = "esd.xlsx"; // Ensure this matches your file name

    var excelService = new ExcelService();
    List<EmployeeModel> employees = null;

    try
    {
        employees = excelService.ReadDataFromExcel(filePath);
    }
    catch (Exception ex)
    {
        LogError($"Error reading Excel file: {ex.Message}");
        WaitForExit();
        return;
    }

    if (employees != null)
    {
        Console.WriteLine($"Total Employees Read: {employees.Count}");
    }

    using (var context = new AppDbContext())
    {
        SeedDatabase(context, employees);
        DisplayData(context);
    }
}
catch (Exception ex)
{
    LogError($"An unexpected error occurred: {ex.Message}");
}
finally
{
    WaitForExit();
}

static void SeedDatabase(AppDbContext context, List<EmployeeModel> employees)
{
    Console.WriteLine("Deleting existing database, if any...");
    context.Database.EnsureDeleted();
    Console.WriteLine("Creating new database...");
    context.Database.EnsureCreated();

    Console.WriteLine("Seeding database with data from Excel...");
    context.Employees.AddRange(employees);
    context.SaveChanges();
    Console.WriteLine("Database seeded.");
}

static void DisplayData(AppDbContext context)
{
    Console.WriteLine("Fetching data from database...");
    var employees = context.Employees.ToList();

    // Create a table
    var table = new Table();

    // Add columns with custom widths
    table.AddColumn(new TableColumn("EEID").Width(10));
    table.AddColumn(new TableColumn("Full Name").Width(20));
    table.AddColumn(new TableColumn("Job Title").Width(20));
    table.AddColumn(new TableColumn("Department").Width(15));
    table.AddColumn(new TableColumn("Business Unit").Width(20));
    table.AddColumn(new TableColumn("Gender").Width(10));
    table.AddColumn(new TableColumn("Ethnicity").Width(15));
    table.AddColumn(new TableColumn("Age").Centered().Width(5));
    table.AddColumn(new TableColumn("Hire Date").Centered().Width(12));
    table.AddColumn(new TableColumn("Annual Salary").RightAligned().Width(15));
    table.AddColumn(new TableColumn("Bonus %").RightAligned().Width(10));
    table.AddColumn(new TableColumn("Country").Width(15));
    table.AddColumn(new TableColumn("City").Width(15));
    table.AddColumn(new TableColumn("Exit Date").Width(12));

    // Add rows
    foreach (var employee in employees)
    {
        table.AddRow(
            employee.EEID,
            employee.FullName,
            employee.JobTitle,
            employee.Department,
            employee.BusinessUnit,
            employee.Gender,
            employee.Ethnicity,
            employee.Age.ToString(),
            employee.HireDate.ToShortDateString(),
            employee.AnnualSalary.ToString("C", new System.Globalization.CultureInfo("en-US")),
            employee.BonusPercentage.ToString("0.00") + "%",
            employee.Country,
            employee.City,
            employee.ExitDate?.ToShortDateString() ?? "N/A"
        );
    }

    // Render the table to the console
    AnsiConsole.Write(table);
}

static void LogError(string message)
{
    var logPath = "error.log";
    File.AppendAllText(logPath, $"{DateTime.Now}: {message}{Environment.NewLine}");
}

static void WaitForExit()
{
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}