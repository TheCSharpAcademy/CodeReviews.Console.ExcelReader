using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace ExcelReader.Doc415;

internal class ExcelFileHandler
{
    static ExcelWorksheet _sheet;
    public static List<string> _colNames = new();
    static List<dynamic> rows;
    private string _excelFile;

    public ExcelFileHandler(string excelFile)
    {
        _excelFile = excelFile;
    }

    public List<string> GetColumnNames()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        FileInfo fi = new FileInfo(_excelFile);
        ExcelPackage excelPackage = new ExcelPackage(fi);
        _sheet = excelPackage.Workbook.Worksheets[0];
     
        string? colName = null;
        bool finished = false;
        Console.WriteLine("Getting column names from Excel data");
        for (int i=1;i<= _sheet.Dimension.End.Column;i++)
        {
            try
            {
                colName = _sheet.Cells[1, i].Value.ToString();
                colName = MakeValidSqlColumnName(colName);
                _colNames.Add(colName);
                _sheet.Cells[1, i].Value = colName;
             
            }
            catch
            {
                finished = true;
            }

        }
        Console.WriteLine($"Sending {_colNames.Count()} column names to ImportService");
        return _colNames;

    }

    public List<dynamic> GetRows()
    {
        Console.WriteLine("Reading row data from worksheet.");
        rows = new();
        for (int i = 2; i <= _sheet.Dimension.End.Row; i++)
        {
            dynamic? row = ReadRow(_sheet, i);
            rows.Add(row);
        }
        Console.WriteLine("Sending row data to Import service.");
        return rows;

    }

    static dynamic ReadRow(ExcelWorksheet worksheet, int rowNumber)
    {
        var headers = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];

        dynamic dynamicModel = new System.Dynamic.ExpandoObject();

        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        {
            var headerText = headers[1, col].Text;
            var cellValue = worksheet.Cells[rowNumber, col].Text;

            ((IDictionary<string, object>)dynamicModel)[headerText] = cellValue;
        }

        return dynamicModel;
    }


    static string MakeValidSqlColumnName(string input)
    {
        string sanitized = Regex.Replace(input, "[^a-zA-Z0-9_]", "");
        if (char.IsDigit(sanitized[0]))
        {
            sanitized = "_" + sanitized;
        }

        return sanitized;
    }
}

