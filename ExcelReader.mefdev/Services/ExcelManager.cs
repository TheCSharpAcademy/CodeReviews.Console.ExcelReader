using ExcelReader.mefdev.Models;
using OfficeOpenXml;

namespace ExcelReader.mefdev.Services;

public class ExcelManager
{
    public ExcelManager()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public List<FinancialData> GetFinancialData(string FilePath)
    {
        List<FinancialData> datas = new();
        FileInfo existingFile = new FileInfo(FilePath);
        using (ExcelPackage package = new ExcelPackage(existingFile))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            int colCount = worksheet.Dimension.End.Column;
            int rowCount = worksheet.Dimension.End.Row;
            for (int row = 2; row <= rowCount; row++)
            {
                var financialData = new FinancialData
                {
                    Account = worksheet.Cells[row, 1].Text?.Trim(),
                    BusinessUnit = worksheet.Cells[row, 2].Text?.Trim(),
                    Currency = worksheet.Cells[row, 3].Text?.Trim(),
                    Year = int.TryParse(worksheet.Cells[row, 4].Text?.Trim(), out int year) ? year : 0,
                    Scenario = worksheet.Cells[row, 5].Text?.Trim(),
                    Jan = ParseCurrencyString(worksheet.Cells[row, 6].Text?.Trim()),
                    Feb = ParseCurrencyString(worksheet.Cells[row, 7].Text?.Trim()),
                    Mar = ParseCurrencyString(worksheet.Cells[row, 8].Text?.Trim()),
                    Apr = ParseCurrencyString(worksheet.Cells[row, 9].Text?.Trim()),
                    May = ParseCurrencyString(worksheet.Cells[row, 10].Text?.Trim()),
                    Jun = ParseCurrencyString(worksheet.Cells[row, 11].Text?.Trim()),
                    Jul = ParseCurrencyString(worksheet.Cells[row, 12].Text?.Trim()),
                    Aug = ParseCurrencyString(worksheet.Cells[row, 13].Text?.Trim()),
                    Sep = ParseCurrencyString(worksheet.Cells[row, 14].Text?.Trim()),
                    Oct = ParseCurrencyString(worksheet.Cells[row, 15].Text?.Trim()),
                    Nov = ParseCurrencyString(worksheet.Cells[row, 16].Text?.Trim()),
                    Dec = ParseCurrencyString(worksheet.Cells[row, 17].Text?.Trim())
                };
                datas.Add(financialData);
            }
            return datas;
        }
    }

    private static decimal ParseCurrencyString(string currencyString)
    {
        if (string.IsNullOrWhiteSpace(currencyString))
            return 0m;

        string cleanedString = currencyString
            .Trim()
            .Replace("$", "")
            .Replace(",", "")
            .Replace("(", "")
            .Replace(")", "");

        if (decimal.TryParse(cleanedString, out decimal result))
        {
            return result;
        }
        throw new FormatException($"Unable to parse currency string: {currencyString}");
    }
}