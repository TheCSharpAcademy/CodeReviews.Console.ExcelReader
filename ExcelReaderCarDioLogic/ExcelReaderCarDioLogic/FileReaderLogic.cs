using OfficeOpenXml;

namespace ExcelReaderCarDioLogic;

internal class FileReaderLogic
{
    public (List<string> headers, List<List<string>> dataRows) ReadExcel(string excelPath)
    {
        Console.WriteLine("Reading the file...");
        Thread.Sleep(1000);

        Console.WriteLine(excelPath);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        try
        {
            using (var package = new ExcelPackage(new FileInfo(excelPath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int rowCount;

                if(worksheet.Dimension != null)
                {
                    rowCount = worksheet.Dimension.Rows;

                    int colCount = worksheet.Dimension.Columns;

                    List<string> headers = new List<string>();

                    for (int col = 1; col <= colCount; col++)
                    {
                        headers.Add(worksheet.Cells[1, col].Text);
                    }

                    List<List<string>> dataRows = new List<List<string>>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        List<string> rowData = new List<string>();
                        for (int col = 1; col <= colCount; col++)
                        {
                            rowData.Add(worksheet.Cells[row, col].Text);
                        }
                        dataRows.Add(rowData);
                    }

                    return (headers, dataRows);
                }
                else 
                {
                    Console.WriteLine("The file is Empty");
                    return (null, null);
                }
            }
        }
        catch
        {
            Console.WriteLine("Error finding the file!");
            Console.ReadLine();
            return (null, null);
        }
    }
}
