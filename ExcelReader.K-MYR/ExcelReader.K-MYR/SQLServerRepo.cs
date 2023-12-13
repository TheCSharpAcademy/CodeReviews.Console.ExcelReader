using Microsoft.Data.SqlClient;
using Spectre.Console;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace ExcelReader.K_MYR;

internal class SQLServerRepo
{
    private readonly string ServerName = @"(LocalDb)\LocalDb.ExcelReader";
    private readonly string DatabaseName = "ExcelReaderDatabase";
    private readonly string ConnectionString = @"Server=(LocalDb)\LocalDb.ExcelReader;Database=ExcelReaderDatabase;Integrated Security = true";
    public readonly List<string> Columns = new();
    public readonly List<SqlDbType> ColumnTypes = new();

    public void CreateDatabase()
    {
        AnsiConsole.Write(new Panel("Creating Database....").BorderColor(Color.SpringGreen2_1));

        try
        {
            using var connection = new SqlConnection($"Server={ServerName};Integrated Security = true");
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = @$" USE master
                                      IF EXISTS (SELECT name FROM sys.databases WHERE name = N'{DatabaseName}' AND name NOT IN ('master', 'tempdb', 'model', 'msdb')) 
                                          BEGIN
                                              ALTER DATABASE {DatabaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                              USE master
                                              DROP DATABASE {DatabaseName}
                                          END                                      
                                      CREATE DATABASE {DatabaseName}";
            command.ExecuteNonQuery();

            AnsiConsole.Write(new Panel("Database Created Sucessfully").BorderColor(Color.SpringGreen2_1));
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Panel($"[red]An Error Occured Creating The Database: {ex.Message}[/]").BorderColor(Color.Red));
            Console.ReadKey();
        }
    }

    public void CreateTable(OfficeOpenXml.ExcelWorksheet ws)
    {
        AnsiConsole.Write(new Panel("Creating Database Tables....").BorderColor(Color.SpringGreen2_1));

        Columns.Clear();
        ColumnTypes.Clear();

        try
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var command = connection.CreateCommand();

            StringBuilder sb = new();
            sb.Append($"USE {DatabaseName} CREATE TABLE TableData (RowId INT IDENTITY(1,1)");

            for (int x = 1; x <= ws.Dimension.End.Column; x++)
            {
                if (ws.Cells[1, x].Value is null || ws.Cells[2, x].Value is null)
                    continue;

                var type = SqlDbTypeMapper.GetSqlDbType(ws.Cells[2, x].Value.GetType());
                ColumnTypes.Add(type);

                var typeString = type.ToString();

                if (typeString == "NVarChar")
                    typeString += "(100)";

                string columnHeader = ws.Cells[1, x].Value.ToString()!.Replace(" ", "_");

                Columns.Add(columnHeader);

                sb.Append($", {columnHeader} {typeString}");
            }

            sb.Append(')');

            command.CommandText = sb.ToString();
            command.ExecuteNonQuery();

            AnsiConsole.Write(new Panel("Database Tables Created Sucessfully").BorderColor(Color.SpringGreen2_1));
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Panel($"[red]An Error Occured Initializing The Database: {ex.Message}[/]").BorderColor(Color.Red));
            Console.ReadKey();
        }
    }

    public System.Data.DataTable? InsertData(OfficeOpenXml.ExcelWorksheet ws)
    {
        AnsiConsole.Write(new Panel("Inserting Data Into Database...").BorderColor(Color.SpringGreen2_1));

        try
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = $"INSERT INTO TableData ({string.Join(", ", Columns)}) VALUES ({string.Join(", ", Enumerable.Range(0, Columns.Count).Select(x => "@" + x))})";

            for (int x = 0; x < Columns.Count; x++)
            {
                command.Parameters.Add(new SqlParameter(x.ToString(), ColumnTypes[x]));
            }

            for (int i = 2; i <= ws.Dimension.End.Row; i++)
            {
                if (ws.Cells[i, 1, i, Columns.Count].All(c => c.Value == null))
                    continue;

                command.Parameters[0].Value = i;

                for (int x = 1; x <= Columns.Count; x++)
                {
                    command.Parameters[x - 1].Value = ws.Cells[i, x].Value ?? DBNull.Value;
                }

                command.ExecuteNonQuery();
            }

            System.Data.DataTable data = new();

            command.CommandText = "SELECT * FROM  TableData";

            SqlDataAdapter da = new(command);
            da.Fill(data);

            AnsiConsole.Write(new Panel("Inserted Data Into Database Sucessfully").BorderColor(Color.SpringGreen2_1));

            return data;
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Panel($"[red]An Error Occured Inserting The Data Into The Database: {ex.Message}[/]").BorderColor(Color.Red));
            return null;
        }
    }

    public System.Data.DataTable? InsertDataWithOleDB(OfficeOpenXml.ExcelWorksheet ws, FileInfo file)
    {
        AnsiConsole.Write(new Panel("Inserting Data Into Database...").BorderColor(Color.SpringGreen2_1));

        try
        {
            string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={file};Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'";
            using OleDbConnection OleDbConnection = new(connectionString);
            OleDbConnection.Open();

            System.Data.DataTable data = new();
            var rowIdCol = data.Columns.Add("RowId", typeof(Int32));

            string sql = $"SELECT * FROM [{ws.Name}$] WHERE ";
            sql += string.Join("OR ", Columns.Select(x => $"[{x.Replace("_", " ")}] IS NOT NULL "));

            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, OleDbConnection);

            adapter.Fill(data);

            OleDbConnection.Close();

            using SqlConnection connection = new(ConnectionString);
            connection.Open();
            using (SqlBulkCopy bulkCopy = new(connection))
            {
                bulkCopy.DestinationTableName = "TableData";
                bulkCopy.WriteToServer(data);
            }

            AnsiConsole.Write(new Panel("Inserted Data Into Database Sucessfully").BorderColor(Color.SpringGreen2_1));

            return data;
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Panel($"[red]An Error Occured Inserting The Data Into The Database: {ex.Message}[/]").BorderColor(Color.Red));
            Console.ReadKey();
            return null;
        }
    }
}
