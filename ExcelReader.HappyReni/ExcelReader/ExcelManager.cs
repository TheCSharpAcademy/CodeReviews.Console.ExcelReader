using ConsoleTableExt;
using ExcelReader.Models;
using OfficeOpenXml;

namespace ExcelReader
{
    public class ExcelManager
    {
        public ExcelService _service;
        private List<ExcelModel> _excelModels;
        private string _relativePath;
        public ExcelManager()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            GetPath();
            _service = new ExcelService(new ExcelContext());
            if (_service._context.Database.CanConnect())
            {
                Console.WriteLine("Database exists.");
                DeleteDB();
            }
            CreateDB();
            Read();
            Add();
            Show();
        }

        public void GetPath()
        {
            try
            {
                string exePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                _relativePath = Path.Combine(exePath, "file.xlsx");

                if (!File.Exists(_relativePath))
                {
                    throw new Exception("No File!");
                }
                Console.WriteLine("Excel file path : " + _relativePath);
            }
            catch
            {
                Console.WriteLine("No excel file found. Quitting...");
                Environment.Exit(0);
            }

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

        public void Read()
        {
            Console.WriteLine("Reading data from Excel File ...");
            using (var package = new ExcelPackage(new FileInfo(_relativePath)))
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
                _excelModels = res;
            }
        }
        public void Add()
        {
            _service.Create(_excelModels);
        }

        public void Show()
        {
            Console.WriteLine("Making the data into table ...");
            List<List<object>> data = _excelModels.Select(e => new List<object>
                                     {
                                         e.Name,
                                         e.Age,
                                         e.Job,
                                         e.Address
                                     }).ToList();
            ConsoleTableBuilder
                .From(data)
                .WithColumn("Name", "Age", "Job", "Address")
                .ExportAndWriteLine();

        }
    }
}
