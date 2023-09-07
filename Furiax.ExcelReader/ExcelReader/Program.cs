using ExcelReader;
using OfficeOpenXml;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//set the projectfolder as workingfolder so the application can find the xlsx file)
var file = new FileInfo("EmployeeSampleData.xlsx");

List<EmployeeModel> employeeList = await LoadExcel.LoadExcelFile(file);
