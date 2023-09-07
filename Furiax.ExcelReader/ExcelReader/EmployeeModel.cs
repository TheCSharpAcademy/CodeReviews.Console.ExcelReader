using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader
{
	internal class EmployeeModel
	{
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string Gender { get; set; }
        public string Ethnicity { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
