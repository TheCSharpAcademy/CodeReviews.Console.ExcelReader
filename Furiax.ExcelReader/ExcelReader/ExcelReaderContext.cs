using Microsoft.EntityFrameworkCore;

namespace ExcelReader
{
	internal class ExcelReaderContext : DbContext
	{
		public DbSet<EmployeeModel> Employees { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb; Database=ExcelReader;Trusted_Connection=True;");
	}
}
