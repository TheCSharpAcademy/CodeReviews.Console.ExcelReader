using ExcelReader.Models;
using OfficeOpenXml;

namespace ExcelReader
{
    public class ExcelManager
    {
        public ExcelService _service;
        private List<ExcelModel> _excelModels;
        public ExcelManager()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _service = new ExcelService(new ExcelContext());
            if (_service._context.Database.CanConnect())
            {
                Console.WriteLine("Database exists.");
                DeleteDB();
            }
            CreateDB();
            _excelModels = Read();
            Add();
        }

        private void DeleteDB()
        {
            Console.WriteLine("Database deleted.");
            _service._context.Database.EnsureDeleted();
        }

        private void CreateDB()
        {
            Console.WriteLine("a new Database created");
            _service._context.Database.EnsureCreated();
        }

        public List<ExcelModel> Read()
        {
            Console.WriteLine("Reading data from Excel File ...");
            using (var package = new ExcelPackage(new FileInfo("file.xlsx")))
            {
                var worksheet = package.Workbook.Worksheets["Sheet1"];
                var rowCount = worksheet.Dimension.End.Row;
                var res = new List<ExcelModel>();

                for (int i = 2; i < rowCount + 1; i++)
                {
                    var name = worksheet.Cells[$"A{i}"].Value.ToString();
                    var age = Int32.Parse(worksheet.Cells[$"B{i}"].Value.ToString());
                    var job = worksheet.Cells[$"C{i}"].Value.ToString();
                    var address = worksheet.Cells[$"D{i}"].Value.ToString();
                    res.Add(new ExcelModel { Name = name, Age = age, Job = job, Address = address });
                }
                Console.WriteLine("Done Reading!");
                return res;
            }
        }
        public void Add()
        {
            _service.Create(_excelModels);
        }
    }
}
