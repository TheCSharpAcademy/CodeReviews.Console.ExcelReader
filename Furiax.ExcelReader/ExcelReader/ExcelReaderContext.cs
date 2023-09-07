using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader
{
	internal class ExcelReaderContext : DbContext
	{
		public DbSet<EmployeeModel> Employees { get; set; }
	}
}
