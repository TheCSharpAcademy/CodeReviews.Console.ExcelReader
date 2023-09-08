using System.ComponentModel.DataAnnotations;

namespace ExcelReader
{
	internal class EmployeeModel
	{
		[Key]
		public string EmployeeId { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string Department { get; set; }
		public string Gender { get; set; }
		public string Age { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
	}
}
