using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ConsoleTableExt;
using Dapper;
using ExcelReader.wkktoria.Model;
using OfficeOpenXml;

namespace ExcelReader.wkktoria;

public static class Program
{
    public static void Main()
    {
        var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        Console.Write("Enter absolute path to .xlsx file: ");
        var filePath = Console.ReadLine();

        if (!new FileInfo(filePath!).Exists)
        {
            Console.WriteLine($"Couldn't find file: {filePath}");
            return;
        }

        var file = new FileInfo(filePath!);

        var data = ReadFromExcel(file);

        CreateTables(connectionString, data);

        ShowDatabaseData(connectionString);
    }

    private static List<Data> ReadFromExcel(FileInfo file)
    {
        Console.WriteLine("Reading data from Excel...");

        var data = new List<Data>();

        using var package = new ExcelPackage(file);

        var worksheet = package.Workbook.Worksheets[0];
        var colCount = worksheet.Dimension.End.Column;
        var rowCount = worksheet.Dimension.End.Row;

        for (var row = 1; row <= rowCount; row++)
        for (var col = 1; col <= colCount; col++)
        {
            var value = worksheet.Cells[row, col].Value?.ToString()!.Trim();

            data.Add(new Data
            {
                RowNum = row,
                ColNum = col,
                Value = value
            });
        }

        return data;
    }

    private static void CreateTables(string connectionString, List<Data> data)
    {
        Console.WriteLine("Creating tables...");

        using var connection = new SqlConnection(connectionString);

        connection.Execute("""
                           DROP DATABASE ExcelData;
                           CREATE DATABASE ExcelData;

                           USE ExcelData;

                           CREATE TABLE Data(Id INT IDENTITY(1,1) PRIMARY KEY);
                           """);

        var columns = data.FindAll(d => d.RowNum == 1).Select(d => d.Value).ToList();
        for (var i = 0; i < columns.Count; i++) columns[i] = $"[{columns[i]}]";
        var columnsString = string.Join(",", columns);

        foreach (var sql in columns.Select(column => $"""
                                                      USE ExcelData;

                                                      IF COL_LENGTH('[Data]', '{column}') IS NULL
                                                       ALTER TABLE Data
                                                       ADD {column} TEXT;
                                                      """))
            connection.Execute(sql);

        var rowCount = data.Select(d => d.RowNum).Max() - 1;

        var values = new List<string>();

        for (var row = 2; row <= rowCount; row++)
        {
            var rowValues = data
                .FindAll(d => d.RowNum == row)
                .Select(d => $"'{d.Value}'")
                .ToList();
            values.Add(string.Join(",", rowValues));
        }

        foreach (var sql in values.Select(value => $"""
                                                    USE ExcelData;

                                                    INSERT INTO Data({columnsString}) VALUES({value});
                                                    """))
            connection.Execute(sql);
    }

    private static void ShowDatabaseData(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);

        var reader = connection.ExecuteReader("""
                                              Use ExcelData;

                                              Select * FROM Data;
                                              """);

        var table = new DataTable();
        table.Load(reader);

        var tableData = new List<List<object>>();
        var columns = new List<string>();

        foreach (DataColumn col in table.Columns) columns.Add(col.ColumnName);

        foreach (DataRow row in table.Rows)
        {
            var rowData = new List<object>();

            foreach (DataColumn col in table.Columns) rowData.Add(row[col]);

            tableData.Add(rowData);
        }

        ConsoleTableBuilder
            .From(tableData)
            .WithColumn(columns)
            .ExportAndWriteLine();
    }
}