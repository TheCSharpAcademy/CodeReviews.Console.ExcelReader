using ExcelReader.Models;
using OfficeOpenXml;
using Spectre.Console;


namespace ExcelReader
{
    internal class Excel
    {
        internal static bool ExcelFileExists(string excelPath)
        {
            return File.Exists(excelPath);
        }
        internal static List<Check> LoadChecks(string excelPath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            List<Check> checks = new List<Check>();

            try
            {
                FileInfo fileInfo = new FileInfo(excelPath);
                using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[0];
                    int rowCount = excelWorksheet.Dimension.End.Row;

                    for (int i = 1; i <= rowCount; i++)
                    {
                        
                        try
                        {
                            string accountNo = excelWorksheet.Cells[i, 1].Value.ToString();
                            int checkNo = int.Parse(excelWorksheet.Cells[i, 2].Value.ToString());
                            decimal amount = decimal.Parse(excelWorksheet.Cells[i, 3].Value.ToString());
                            DateTime dateTime = DateTime.Parse(excelWorksheet.Cells[i, 4].Value.ToString());
                            DateOnly date = DateOnly.FromDateTime(dateTime);
                            string canceledString = excelWorksheet.Cells[i, 5].Value.ToString().ToLower();
                            bool canceled = canceledString == "y";

                            Check newCheck = new Check(accountNo, checkNo, amount, date, canceled);

                            checks.Add(newCheck);
                        }
                        catch (Exception ex) 
                        {
                            string header = excelWorksheet.Cells[i, 1].Value.ToString().ToLower();
                            if (i == 1 && header.Contains("account"))
                            {
                                AnsiConsole.MarkupLine("LOG: Header row detected.");
                            }
                            else
                            {
                                Console.WriteLine(@$"Failed to load row {i}");
                                Console.WriteLine(ex.Message);
                                Console.WriteLine(ex.StackTrace);
                                Console.WriteLine();
                            }
                        }
                    } 
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(@$"Exception: {ex}");
            }

            return checks;
        }
    }
}
