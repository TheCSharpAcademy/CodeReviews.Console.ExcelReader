using ExcelReader.Forser.UI;
using OfficeOpenXml;

namespace ExcelReader.Forser.Controllers
{
    internal class ExcelController : IExcelController
    {
        private readonly IExcelService _excelService;
        private readonly IUserInterface _excelUI;
        bool isImportDone;

        public ExcelController(IExcelService excelService, IUserInterface excelUI)
        {
            _excelService = excelService;
            _excelUI = excelUI;
        }
        public void Run()
        {
            if (_excelService == null)
            {
                throw new ArgumentNullException();
            }

            Console.WriteLine("Read EXCEL file and import to DB");
            ImportExcelData();
            if(isImportDone)
            {
                Console.WriteLine("Displaying Table from Database");
                DisplayAllPlayers();
            }
        }
        public void DisplayAllPlayers()
        {
            _excelUI.RenderTitle("Hockey Players");
            _excelUI.DisplayAllPlayers(_excelService.DisplayAllPlayers());
        }
        public void ImportExcelData()
        {
            string excelFile = @"Data\sampledatahockey.xlsx";

            List<HockeyModel> hockeyPlayers = new List<HockeyModel>();
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFile)))
            {
                try
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets["PlayerData"];
                    if(workSheet != null)
                    {
                        int rowCount = workSheet.Dimension.Rows;

                        for (int row = 4; row <= rowCount; row++)
                        {
                            hockeyPlayers.Add(new HockeyModel
                            {
                                Id = Convert.ToInt32(workSheet.Cells[row, 1].Value),
                                Team = workSheet.Cells[row, 2].Value.ToString().Trim(),
                                Country = workSheet.Cells[row, 3].Value.ToString().Trim(),
                                FirstName = workSheet.Cells[row, 4].Value.ToString().Trim(),
                                LastName = workSheet.Cells[row, 5].Value.ToString().Trim(),
                                Weight = Convert.ToInt32(workSheet.Cells[row, 6].Value),
                                Height = workSheet.Cells[row, 7].Value.ToString().Trim(),
                                DateOfBirth = workSheet.Cells[row, 8].Value.ToString().Trim(),
                                HomeTown = workSheet.Cells[row, 9].Value.ToString().Trim(),
                                Position = workSheet.Cells[row, 10].Value.ToString().Trim(),
                                Provinces = workSheet.Cells[row, 11].Value.ToString().Trim(),
                                Age = Convert.ToInt32(workSheet.Cells[row, 12].Value),
                                HeightFt = workSheet.Cells[row, 13].Value.ToString().Trim(),
                                Htln = float.Parse(workSheet.Cells[row, 14].Value.ToString().Trim()),
                                Bmi = Convert.ToInt32(workSheet.Cells[row, 15].Value)
                            });
                        }

                        _excelService.AddPlayers(hockeyPlayers);
                        isImportDone = true;
                    }
                    else
                    {
                        Console.WriteLine("Couldn't find the Excel File");
                    }
                }
                catch (Exception ex)
                {
                    Console.Write($"Couldn't parse the Excel File. Error Message: {ex.Message}");
                }
            }
        }
    }
}
