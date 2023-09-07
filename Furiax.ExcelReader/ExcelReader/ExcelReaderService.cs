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
	}
}
