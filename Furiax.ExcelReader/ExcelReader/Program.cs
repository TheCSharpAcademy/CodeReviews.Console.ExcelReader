using ExcelReader;
using OfficeOpenXml;


ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//set the projectfolder as workingfolder so the application can find the xlsx file)
var file = new FileInfo("EmployeeSampleData.xlsx");

Console.WriteLine("Reading the Excel file ...");
List<EmployeeModel> employeeList = await ExcelReaderService.LoadExcelFile(file);
Thread.Sleep(1000);

using (var context = new ExcelReaderContext())
{
	Console.WriteLine("Deleting database (in case it already exists)...");
	context.Database.EnsureDeleted();
	Thread.Sleep(1000);
	Console.WriteLine("Creating the database ...");
	context.Database.EnsureCreated();
	Thread.Sleep(1000);

	Console.WriteLine("Populating the database with the data ...");
	foreach (EmployeeModel employee in employeeList)
	{
		ExcelReaderService.AddData(employee);
	}
	Thread.Sleep(2000);
}
Console.Clear();
ExcelReaderService.PrintTable();