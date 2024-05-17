using OfficeOpenXml;

namespace STUDY.ConsoleP.ExcelReaderTryTwo;
internal class ExcelDataReader
{
    public List<ExcelDbModel> ReadExcelFile(string filePath)
    {
        List<ExcelDbModel> excelDataList = new List<ExcelDbModel>();
            
        try
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; 

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++) 
                {                    
                    string firstName = worksheet.Cells[row, 2].Value?.ToString();
                    string lastName = worksheet.Cells[row, 3].Value?.ToString();
                                        
                    ExcelDbModel excelData = new ExcelDbModel
                    {
                        FirstName = firstName,
                        LastName = lastName
                    };
                                        
                    excelDataList.Add(excelData);

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
