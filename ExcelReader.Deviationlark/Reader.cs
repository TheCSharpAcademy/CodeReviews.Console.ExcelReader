using OfficeOpenXml;

namespace ExcelReader;

public class Reader
{
    public void ExcelReader()
    {
        Console.WriteLine("Reading Info from Excel table");
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        FileInfo file = new FileInfo("GymInfo.xlsx");
        Gym gymInfo = new();

        using (var package = new ExcelPackage(file))
        {
            var worksheet = package.Workbook.Worksheets["Sheet1"];
            string day = "";
            string exercises = "";
            for (int row = 2; row <= worksheet.Rows.Count(); row++)
            {
                int col = 1;
                gymInfo.Monday = "";
                gymInfo.Tuesday = "";
                gymInfo.Wednesday = "";
                gymInfo.Thursday = "";
                gymInfo.Friday = "";
                gymInfo.Saturday = "";
                gymInfo.Sunday = "";

                gymInfo.Monday = worksheet.Cells[row, col++].GetValue<string>();
                gymInfo.Tuesday = worksheet.Cells[row, col++].GetValue<string>();
                gymInfo.Wednesday = worksheet.Cells[row, col++].GetValue<string>();
                gymInfo.Thursday = worksheet.Cells[row, col++].GetValue<string>();
                gymInfo.Friday = worksheet.Cells[row, col++].GetValue<string>();
                gymInfo.Saturday = worksheet.Cells[row, col++].GetValue<string>();
                gymInfo.Sunday = worksheet.Cells[row, col++].GetValue<string>();
                Program.Insert(gymInfo);
            }
        }
        Console.WriteLine("Writing Info to database");
    }
}