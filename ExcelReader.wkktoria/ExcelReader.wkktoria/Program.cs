using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ConsoleTableExt;
using Dapper;
using ExcelReader.wkktoria.Model;
using OfficeOpenXml;
using Spectre.Console;

namespace ExcelReader.wkktoria;

public static class Program
{
    public static void Main()
    {
        var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        string? filePath = null;

        if (AnsiConsole.Confirm("Do you want to use one of default spreadsheets?"))
        {
            var path = Path.GetDirectoryName(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                ?.Parent?.Parent?.Parent?
                .FullName);

            var spreadsheet = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Choose spreadsheet:")
                .AddChoices("Programming Languages", "Employee Data"));

            filePath = spreadsheet switch
            {
                "Programming Languages" => $"{path}/ProgrammingLanguages.xlsx",
                "Employee Data" => $"{path}/EmployeeSampleData.xlsx",
                _ => filePath
            };
        }
        else
        {
            filePath = AnsiConsole.Ask<string>("Enter absolute path to .xlsx file:");
        }


        if (!new FileInfo(filePath!).Exists)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Couldn't find file: {filePath}[/]");
            return;
        }

        var file = new FileInfo(filePath!);

        var data = ReadFromExcel(file);

        CreateTables(connectionString, data);

        ShowDatabaseData(connectionString);
    }

    private static List<Data> ReadFromExcel(FileInfo file)
    {
        AnsiConsole.MarkupLine("[yellow]Reading data from spreadsheet...[/]");

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
                Value = value
            });
        }

        return data;
    }

    private static void CreateTables(string connectionString, List<Data> data)
    {
        AnsiConsole.MarkupLine("[yellow]Creating tables...[/]");

        CleanUpDatabase(connectionString);

        using var connection = new SqlConnection(connectionString);

        connection.Execute("""
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

        connection.Close();
    }

    private static void CleanUpDatabase(string connectionString)
    {
        AnsiConsole.MarkupLine("[yellow]Creating tables...[/]");

        using var connection = new SqlConnection(connectionString);

        connection.Execute("""
                           IF DB_ID('ExcelData') IS NOT NULL
                            DROP DATABASE ExcelData;
                            
                           CREATE DATABASE ExcelData;
                           """);

        connection.Close();
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

        connection.Close();

        var columns = (from DataColumn col in table.Columns select col.ColumnName).ToList();

        var tableData = (from DataRow row in table.Rows
            select (from DataColumn col in table.Columns select row[col]).ToList()).ToList();

        ConsoleTableBuilder
            .From(tableData)
            .WithColumn(columns)
            .ExportAndWriteLine();
    }
}