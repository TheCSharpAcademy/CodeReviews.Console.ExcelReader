using ExcelReader;
using OfficeOpenXml;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//string filePath = "EmployeeSampleData.xlsx";
var file = new FileInfo("EmployeeSampleData.xlsx");

List<EmployeeModel> employeeList = await LoadExcel.LoadExcelFile(file);

int counter = 1;
foreach (var employee in employeeList)
{
	Console.WriteLine($"{counter}. {employee.EmployeeId} {employee.Name}");
	counter++;
}
