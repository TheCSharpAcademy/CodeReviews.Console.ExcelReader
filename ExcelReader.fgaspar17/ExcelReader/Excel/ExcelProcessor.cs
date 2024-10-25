using Spectre.Console;

namespace ExcelReader;

public class ExcelProcessor(string excelPath)
{
    private readonly FileInfo _fileInfo = new FileInfo(excelPath);
    private readonly ExcelReader _excelReader = new ExcelReader();

    public void Run()
    {
        int worksheetsCount = _excelReader.GetWorkSheetsCount(_fileInfo);
        for (int i = 0; i < worksheetsCount; i++)
        {
            // Create the table
            AnsiConsole.WriteLine($"Creating table for the Worksheet {i + 1} ...");
            (bool tableCreated, string tableName) = CreateTable(i);
            if (!tableCreated) continue;

            // Get the data from the worksheet
            AnsiConsole.WriteLine($"Reading the data from the Worksheet {i + 1} ...");
            var columnValues = GetColumnValues(i);

            // Insert the data to the database
            AnsiConsole.WriteLine($"Inserting the data for the Worksheet {i + 1} ...");
            InsertData(columnValues, tableName);

            AnsiConsole.WriteLine();
        }

        ExcelMenuHandler excelMenuHandler = new(_excelReader.GetWorkSheetName(_fileInfo, 0));
        excelMenuHandler.Display();
    }

    private Dictionary<string, List<string>> GetColumnValues(int worksheetIndex)
    {
        return _excelReader.GetData(_fileInfo, worksheetIndex);
    }

    private (bool IsCreated, string TableName) CreateTable(int worksheetIndex)
    {
        var columns = _excelReader.GetColumns(_fileInfo, worksheetIndex);
        if (columns.Count == 0) return (false, null);
        string worksheetName = _excelReader.GetWorkSheetName(_fileInfo, worksheetIndex);
        return (SetupDatabase.CreateTable(worksheetName, columns), worksheetName);
    }

    private void InsertData(Dictionary<string, List<string>> columnValues, string tableName)
    {
        var rows = new List<Dictionary<string, string>>();

        var firstRow = columnValues.Values.FirstOrDefault();
        if (firstRow == null) return;
        int rowCount = firstRow.Count;

        for (int r = 0; r < rowCount; r++)
        {
            var row = new Dictionary<string, string>();
            foreach (var column in columnValues.Keys)
            {
                row.Add(column, columnValues[column][r]);
            }
            rows.Add(row);
        }

        foreach (var row in rows)
        {
            ExcelController.InsertData(tableName, row, GlobalConfig.ConnectionString);
        }
    }
}