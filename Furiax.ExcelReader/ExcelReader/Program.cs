using ExcelReader;
using OfficeOpenXml;
using System.Drawing.Text;

	ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

	//changed workingfolder to ExcelReader
	string filePath = "EmployeeSampleData.xlsx";
	var file = new FileInfo(filePath);	

	List<EmployeeModel> employeeList = await LoadExcelFile(file);

int counter = 1;
	foreach (var employee in employeeList)
	{
        Console.WriteLine($"{counter}. {employee.EmployeeId} {employee.Name}");
	counter++;
    }

static async Task<List<EmployeeModel>> LoadExcelFile(FileInfo file)
{
	List<EmployeeModel> output = new();
	using var package = new ExcelPackage(file);
	await package.LoadAsync(file);
	
	var ws  = package.Workbook.Worksheets[0];

	int row = 2;
	int col = 1;

	while (string.IsNullOrWhiteSpace(ws.Cells[row,col].Value?.ToString()) == false)
	{
		EmployeeModel emp = new();
		emp.EmployeeId = ws.Cells[row,col].Value.ToString();
		emp.Name = ws.Cells[row, col+1].Value.ToString();
		emp.Title = ws.Cells[row, col+2].Value.ToString();
		emp.Department = ws.Cells[row, col+3].Value.ToString();
		emp.Gender = ws.Cells[row, col+4].Value.ToString();
		emp.Ethnicity = ws.Cells[row, col+5].Value.ToString();
		emp.Age = int.Parse(ws.Cells[row, col+6].Value.ToString());
		emp.Country = ws.Cells[row, col+7].Value.ToString();
		emp.City = ws.Cells[row, col+8].Value.ToString();
		output.Add(emp);
		row++;
	}
	return output;
}

