using ExcelReader;
using OfficeOpenXml;


ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//set the projectfolder as workingfolder so the application can find the xlsx file)
var file = new FileInfo("EmployeeSampleData.xlsx");

List<EmployeeModel> employeeList = await ExcelReaderService.LoadExcelFile(file);

using (var context = new ExcelReaderContext())
{
	context.Database.EnsureDeleted();
	context.Database.EnsureCreated();

	foreach (EmployeeModel employee in employeeList)
	{
		ExcelReaderService.AddData(employee);
	}
}

ExcelReaderService.PrintTable();