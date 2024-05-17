using OfficeOpenXml;
using STUDY.ConsoleP.ExcelReader.Models;

namespace STUDY.ConsoleP.ExcelReader;
internal class ExcelDataReader
{
    public List<ExcelDataModel> ReadExcelFile(string filePath)
    {
        List<ExcelDataModel> excelDataList = new List<ExcelDataModel>();

            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming first worksheet

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    for (int row = 2; row <= rowCount; row++) // Assuming data starts from row 2
                    {
                        // Read data from Excel file and create ExcelDataModel objects
                        string firstName = worksheet.Cells[row, 1].Value?.ToString();
                        string lastName = worksheet.Cells[row, 2].Value?.ToString();

                        // Create ExcelDataModel object
                        ExcelDataModel excelData = new ExcelDataModel
                        {
                            FirstName = firstName,
                            LastName = lastName
                        };

                        // Add the ExcelDataModel object to the list
                        excelDataList.Add(excelData);

                        // For demonstration purposes, printing to console
                        Console.WriteLine($"First Name: {excelData.FirstName}, Last Name: {excelData.LastName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading Excel file: {ex.Message}");
            }

            return excelDataList;
    }
}
