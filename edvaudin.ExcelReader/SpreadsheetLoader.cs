using OfficeOpenXml;
using System.Data;

namespace edvaudin.ExcelReader;

internal static class SpreadsheetLoader
{
    public static async Task<DataTable> LoadExcelFile(FileInfo file)
    {
        DataTable output = new();

        using var package = new ExcelPackage(file);

        await package.LoadAsync(file);

        var ws = package.Workbook.Worksheets[0];

        CreateIdColumn(output, ws);

        SetIdColumnAsPK(output);

        DataSet set = new();
        set.Tables.Add(output);

        AddRows(output, ws);

        return output;
    }

    private static void SetIdColumnAsPK(DataTable output)
    {
        DataColumn[] PrimaryKeyColumns = new DataColumn[1];
        PrimaryKeyColumns[0] = output.Columns["id"];
        output.PrimaryKey = PrimaryKeyColumns;
    }

    private static void CreateIdColumn(DataTable output, ExcelWorksheet ws)
    {
        DataColumn column = new();
        column.DataType = System.Type.GetType("System.Int32");
        column.ColumnName = "Id";
        column.ReadOnly = true;
        column.Unique = true;
        output.Columns.Add(column);
        SetupDataTableColumns(output, ws);
    }

    private static void SetupDataTableColumns(DataTable input, ExcelWorksheet ws)
    {
        int row = 1;
        int col = 2;

        while (string.IsNullOrWhiteSpace(ws.Cells[row, col].Value?.ToString()) == false)
        {
            AddNonPrimaryColumn(input, ws, row, col);
            col++;
        }
    }

    private static void AddNonPrimaryColumn(DataTable input, ExcelWorksheet ws, int row, int col)
    {
        DataColumn dataColumn = new();
        dataColumn.DataType = System.Type.GetType("System.String");
        dataColumn.ColumnName = ws.Cells[row, col].Value.ToString();
        dataColumn.AutoIncrement = false;
        dataColumn.Unique = false;
        dataColumn.ReadOnly = false;
        input.Columns.Add(dataColumn);
    }

    private static void AddRows(DataTable input, ExcelWorksheet ws)
    {
        int row = 2;
        int col = 1;
        while (string.IsNullOrWhiteSpace(ws.Cells[row, col].Value?.ToString()) == false)
        {
            DataRow dataRow;
            dataRow = input.NewRow();
            foreach (DataColumn dataColumn in input.Columns)
            {
                int index = input.Columns.IndexOf(dataColumn) + 1;
                dataRow[dataColumn.ColumnName] = dataColumn.Unique == true ? int.Parse(ws.Cells[row, index].Value.ToString()) : ws.Cells[row, index].Value.ToString();
            }
            row++;
            input.Rows.Add(dataRow);
        }
    }
}
