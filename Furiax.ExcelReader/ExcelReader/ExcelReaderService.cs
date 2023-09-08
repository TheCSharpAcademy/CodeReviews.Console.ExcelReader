using Spectre.Console;
using OfficeOpenXml;

namespace ExcelReader
{
	internal class ExcelReaderService
	{
		internal static void AddData(EmployeeModel employee)
		{
			using var db = new ExcelReaderContext();
			db.Add(employee);
			db.SaveChanges();
		}

		public static async Task<List<EmployeeModel>> LoadExcelFile(FileInfo file)
		{
			List<EmployeeModel> output = new();
			using var package = new ExcelPackage(file);
			await package.LoadAsync(file);
			var worksheet = package.Workbook.Worksheets[0];

			int row = 2;
			int col = 1;

			try
			{
				while (string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Value?.ToString()) == false)
				{
					EmployeeModel emp = new();
					emp.EmployeeId = worksheet.Cells[row, col].Value.ToString();
					emp.Name = worksheet.Cells[row, col + 1].Value.ToString();
					emp.Title = worksheet.Cells[row, col + 2].Value.ToString();
					emp.Department = worksheet.Cells[row, col + 3].Value.ToString();
					emp.Gender = worksheet.Cells[row, col + 4].Value.ToString();
					emp.Age = worksheet.Cells[row, col + 5].Value.ToString();
					emp.Country = worksheet.Cells[row, col + 6].Value.ToString();
					emp.City = worksheet.Cells[row, col + 7].Value.ToString();
					output.Add(emp);
					row++;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Something went wrong reading the Excel file: {ex.Message}");
			}
			return output;
		}

		internal static void PrintTable()
		{
			using var db = new ExcelReaderContext();
			var employeeList = db.Employees.ToList();

			var table = new Table();
			table.AddColumn("EmployeeId");
			table.AddColumn("Name");
			table.AddColumn("Title");
			table.AddColumn("Department");
			table.AddColumn("Gender");
			table.AddColumn("Age");
			table.AddColumn("Country");
			table.AddColumn("City");

			foreach (var employee in employeeList)
			{
				table.AddRow(employee.EmployeeId,
					employee.Name,
					employee.Title,
					employee.Department,
					employee.Gender,
					employee.Age,
					employee.Country,
					employee.City);
			}
			AnsiConsole.Write(table);
			Console.ReadKey();
		}
	}
}
