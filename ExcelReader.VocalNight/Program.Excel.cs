using ConsoleTableExt;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System.Configuration;
using System.Data;

partial class Program
{

    static DataTable LoadExcelData( string filePath )
    {
        using (var package = new OfficeOpenXml.ExcelPackage(new System.IO.FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0];
            DataTable dt = new DataTable();

            // Add columns dynamically based on Excel headers
            foreach (var headerCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
            {
                dt.Columns.Add(headerCell.Text);
            }

            // Iterate through rows and populate data
            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var dataRow = dt.Rows.Add();
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    dataRow[col - 1] = worksheet.Cells[row, col].Text;
                }
            }

            return dt;
        }
    }

    static DataTable GetHeaders( string filePath )
    {
        DataTable excelSchema = new DataTable();

        using (ExcelPackage package = new ExcelPackage(filePath))
        {
            var worksheet = package.Workbook.Worksheets[0];

            foreach (var header in worksheet.Cells["A1:Z1"])
            {
                excelSchema.Columns.Add(header.Text);
            }
        }
        return excelSchema;
    }

    static void ShowTable()
    {

        DataTable resultTable = new DataTable();
        using (var connection = new SqlConnection(ConfigurationManager.AppSettings.Get("DatabaseConnectionString")))
        {
            connection.Open();

            using (var command = new SqlCommand("Select * from FileRead", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    // Create columns in the result DataTable dynamically based on the database schema
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        resultTable.Columns.Add(reader.GetName(i), typeof(string));
                    }

                    // Populate the result DataTable with data
                    while (reader.Read())
                    {
                        var dataRow = resultTable.NewRow();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dataRow[i] = reader[i].ToString();
                        }

                        resultTable.Rows.Add(dataRow);

                    }
                }
            }
        }

        var tableBuilder = ConsoleTableBuilder.From(resultTable)
            .WithFormat(ConsoleTableBuilderFormat.Minimal);

        // Print the table to the console
        tableBuilder.ExportAndWriteLine();
    }

    void DisplayResult( DataTable resultTable )
    {
        foreach (DataColumn column in resultTable.Columns)
        {
            Console.Write($"{column.ColumnName}\t");
        }
        Console.WriteLine();

        foreach (DataRow row in resultTable.Rows)
        {
            foreach (var item in row.ItemArray)
            {
                Console.Write($"{item}\t");
            }
            Console.WriteLine();
        }
    }

    static void PopulateTable( string file, string connectionString, DataTable columnHeaders, string databaseName )
    {
        using (ExcelPackage package = new ExcelPackage(file))
        {

            var worksheet = package.Workbook.Worksheets[0];
            DataTable excelData = columnHeaders;

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var dataRow = excelData.NewRow();

                // Populate dataRow with values from Excel
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    // Use the column name from excelSchema to get the correct index
                    var columnName = columnHeaders.Columns[col - 1].ColumnName;
                    dataRow[columnName] = worksheet.Cells[row, col].Text;

                }

                excelData.Rows.Add(dataRow);
            }


            // Insert data into SQL Server
            using (var bulkCopy = new SqlBulkCopy(ConfigurationManager.AppSettings.Get("DatabaseConnectionString")))
            {
                bulkCopy.DestinationTableName = "FileRead";
                bulkCopy.WriteToServer(excelData);
            }
        }
    }
}

