using ExcelReader.Database;
using ExcelReader.Model;
using OfficeOpenXml;

namespace ExcelReader.Service
{
    public class Service
    {
        public static string filePath;
        private Context context;
        public Service()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            context = new Context();
        }
        
        public void Initialize()
        {
            SetFilePath();
            WorkFlow();
            Console.WriteLine("\nPress any button to close the app.");
            Console.ReadLine();
        }

        public void SetFilePath()
        {
            Console.WriteLine("Setting filepath to:");
            try
            {
                string currentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                filePath = Path.Combine(currentPath, "Book.xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to close the app.");
                Console.ReadLine();
                Environment.Exit(0);
            }
            Console.WriteLine(filePath);
        }

        public void WorkFlow()
        {
            RemoveDB();
            CreateDB();
            ReadData();
            DisplayData();
        }

        private void RemoveDB()
        {
            try
            {
                context.Database.EnsureDeleted();
                Console.WriteLine("Database removed successfully if there was one.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CreateDB()
        {
            try
            {
                context.Database.EnsureCreated();
                Console.WriteLine("Database has been created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DisplayData()
        { 
            List<ExcelData> list = new List<ExcelData>();
            try
            {
                list = context.ExcelDbSet.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (!list.Any())
            {
                Console.WriteLine("No data found in the database.");
                return;
            }
            Console.Clear();
            foreach (ExcelData data in list)
            {
                Console.WriteLine($"Item's name: {data.Name}. Item's price: {data.Price}");
            }
        }

        private void ReadData()
        {
            Console.WriteLine($"Trying to read data from {filePath}");
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                try
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                    {
                        var excelData = new ExcelData()
                        {
                            Name = worksheet.Cells[i, 1].Text,
                            Price = decimal.TryParse(worksheet.Cells[i, 2].Text, out var price) ? price : 0
                        };
                        context.ExcelDbSet.Add(excelData);
                    }
                    context.SaveChanges();
                    Console.WriteLine("Database updated successfully.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}