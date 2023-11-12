using OfficeOpenXml;

namespace ExcelReader.Models;
public static class ReadExcelData
{
    public static List<Car> Read()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        string file = @"Data\Cars.xlsx";

        FileInfo existingFile = new FileInfo(file);

        List<Car> Cars = new List<Car>();

        using (ExcelPackage package = new ExcelPackage(existingFile))
        {
            //Get the first worksheet in the workbook
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];


            int colMake = 2;
            int colModel = 3;
            int colHP = 4;
            int colYear = 5;

            for (int row = 2; row <= 8; row++)
            {
                var make = worksheet.Cells[row, colMake].Value;
                var model = worksheet.Cells[row, colModel].Value;
                var hp = worksheet.Cells[row, colHP].Value;
                var year = worksheet.Cells[row, colYear].Value;

                Console.WriteLine($"{make} {model} {hp} {year}");

                Cars.Add(new Car
                {
                    //Id = Convert.ToInt32(id),
                    Make = make.ToString(),
                    Model = model.ToString(),
                    HP = Convert.ToInt32(hp),
                    Year = Convert.ToInt32(year)
                });
            }
        }

        return Cars;
    }
}

