using OfficeOpenXml;

namespace ExcelReader;

public class ExcelReader
{
    public ExcelReader()
    {

    }

    public int GetWorkSheetsCount(FileInfo fileInfo)
    {
        using ExcelPackage package = new(fileInfo);

        // Get the number of worksheets in the workbook
        return package.Workbook.Worksheets.Count;
    }

    public string GetWorkSheetName(FileInfo fileInfo, int worksheetIndex)
    {
        using ExcelPackage package = new(fileInfo);

        // Get the name of the worksheet index in the workbook
        return package.Workbook.Worksheets[worksheetIndex].Name;
    }

    public List<string> GetColumns(FileInfo fileInfo, int worksheetIndex)
    {
        var columns = new List<string>();

        using ExcelPackage package = new(fileInfo);

        // Get the first worksheet in the workbook
        ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheetIndex];

        int row = 1;
        // Get the dimensions of the worksheet columns
        int colCount = worksheet.Dimension.Columns;

        // Loop through the columns of the first row
        for (int col = 1; col <= colCount; col++)
        {
            columns.Add(worksheet.Cells[row, col].Text);
        }


        return columns;
    }

    public Dictionary<string, List<string>> GetData(FileInfo fileInfo, int worksheetIndex)
    {
        var columnValues = new Dictionary<string, List<string>>();
        var columns = GetColumns(fileInfo, worksheetIndex);
        foreach (var column in columns)
        {
            columnValues.Add(column, new());
        }

        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            // Get the first worksheet in the workbook
            ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheetIndex];

            // Get the dimensions of the worksheet columns and rows
            int colCount = worksheet.Dimension.Columns;
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                // Loop through the columns of the first row
                for (int col = 1; col <= colCount; col++)
                {
                    columnValues[columns[col - 1]].Add(worksheet.Cells[row, col].Text);
                }
            }
        }

        return columnValues;
    }
}