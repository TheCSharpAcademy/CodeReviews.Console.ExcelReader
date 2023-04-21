using ExcelReader.Model;
using OfficeOpenXml;

namespace ExcelReader;

public class Reader
{
    private readonly string FilePath;

    public Reader(string filePath)
    {
        FilePath = filePath;
    }

    public List<Aliment> ParseFile()
    {
        List<Aliment> aliments = new();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        FileInfo existingFile = new FileInfo(FilePath);

        using (ExcelPackage package = new(existingFile))
        {
            var sheet = package.Workbook.Worksheets.First();

            int rowCount = sheet.Dimension.End.Row;

            for (int row = 2; row <= rowCount; row++)
            {
                aliments.Add(new()
                {
                    Name = sheet.Cells[row, 1].Value?.ToString(),
                    DailyDosage = sheet.Cells[row, 2].Value?.ToString(),
                    Calories = sheet.Cells[row, 3].Value?.ToString(),
                    Proteines = sheet.Cells[row, 4].Value?.ToString(),
                    DailyCalories = sheet.Cells[row, 5].Value?.ToString(),
                    DailyProteines = sheet.Cells[row, 6].Value?.ToString()
                });
            }
        }

        return aliments;
    }
}