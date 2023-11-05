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
        CreateDatabase(connectionString);

        using var connection = new SqlConnection(connectionString);


        var columns = data.FindAll(d => d.RowNum == 1).Select(d => d.Value).ToList();
        for (var i = 0; i < columns.Count; i++) columns[i] = $"[{columns[i]}]";

        CreateDatabaseColumns(connectionString, columns);

        var columnsString = string.Join(",", columns);

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

        InsertDataIntoDatabase(connectionString, values, columnsString);

        connection.Close();
    }

    private static void CreateDatabase(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);

        try
        {
            connection.Execute("""
                               USE ExcelData;

                               CREATE TABLE Data(Id INT IDENTITY(1,1) PRIMARY KEY);
                               """);
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Couldn't create database: {e.Message}[/]");
        }
        finally
        {
            if (connection.State == ConnectionState.Open) connection.Close();
        }
    }

    private static void CreateDatabaseColumns(string connectionString, IEnumerable<string?> columns)
    {
        using var connection = new SqlConnection(connectionString);

        try
        {
            foreach (var sql in columns.Select(column => $"""
                                                          USE ExcelData;

                                                          IF COL_LENGTH('[Data]', '{column}') IS NULL
                                                           ALTER TABLE Data
                                                           ADD {column} TEXT;
                                                          """))
                connection.Execute(sql);
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Couldn't create database columns: {e.Message}[/]");
        }
        finally
        {
            if (connection.State == ConnectionState.Open) connection.Close();
        }
    }

    private static void InsertDataIntoDatabase(string connectionString, IEnumerable<string> values,
        string columnsString)
    {
        using var connection = new SqlConnection(connectionString);

        try
        {
            foreach (var sql in values.Select(value => $"""
                                                        USE ExcelData;

                                                        INSERT INTO Data({columnsString}) VALUES({value});
                                                        """))
                connection.Execute(sql);
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Couldn't insert data into database: {e.Message}[/]");
        }
        finally
        {
            if (connection.State == ConnectionState.Open) connection.Close();
        }
    }

    private static void CleanUpDatabase(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);

        try
        {
            connection.Execute("""
                               IF DB_ID('ExcelData') IS NOT NULL
                                DROP DATABASE ExcelData;
                                
                               CREATE DATABASE ExcelData;
                               """);
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Couldn't clean up database: {e.Message}[/]");
        }
        finally
        {
            if (connection.State == ConnectionState.Open) connection.Close();
        }
    }

    private static DataTable CreateDataTable(string connectionString)
    {
        var dataTable = new DataTable();
        using var connection = new SqlConnection(connectionString);

        try
        {
            var reader = connection.ExecuteReader("""
                                                  Use ExcelData;

                                                  Select * FROM Data;
                                                  """);

            dataTable.Load(reader);
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Couldn't create data table: {e.Message}[/]");
        }
        finally
        {
            if (connection.State == ConnectionState.Open) connection.Close();
        }

        return dataTable;
    }

    private static void ShowDatabaseData(string connectionString)
    {
        var table = CreateDataTable(connectionString);

        var columns = (from DataColumn col in table.Columns select col.ColumnName).ToList();

        var tableData = (from DataRow row in table.Rows
            select (from DataColumn col in table.Columns select row[col]).ToList()).ToList();

        ConsoleTableBuilder
            .From(tableData)
            .WithColumn(columns)
            .ExportAndWriteLine();
    }
}