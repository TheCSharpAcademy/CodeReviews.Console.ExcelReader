using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edvaudin.ExcelReader
{
    internal class SpreadsheetLoader
    {
        public static async Task<DataTable> LoadExcelFile(FileInfo file)
        {
            DataTable output = new();

            using var package = new ExcelPackage(file);

            await package.LoadAsync(file);

            var ws = package.Workbook.Worksheets[0];

            int row = 1;
            int col = 1;

            while (string.IsNullOrWhiteSpace(ws.Cells[row, col].Value?.ToString()) == false)
            {
                row++;
            }
            DataColumn column = new();

            // Create Id column
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "Id";
            column.ReadOnly = true;
            column.Unique = true;
            output.Columns.Add(column);
            SetupDataTableColumns(output, ws);

            // Set Id Column as primary key
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = output.Columns["id"];
            output.PrimaryKey = PrimaryKeyColumns;

            DataSet set = new DataSet();
            set.Tables.Add(output);

            AddRows(output, ws);

            return output;
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
                    Console.WriteLine($"Attempting to assign the value of {dataColumn.ColumnName} to row {row} column {index}. Is it unique? {dataColumn.Unique}");
                    dataRow[dataColumn.ColumnName] = dataColumn.Unique == true ? int.Parse(ws.Cells[row, index].Value.ToString()) : ws.Cells[row, index].Value.ToString();
                }
                row++;
                input.Rows.Add(dataRow);
            }
        }
    }
}
