﻿using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace edvaudin.ExcelReader;

internal class DbFactory
{
    private readonly string connectionString = ConfigurationManager.ConnectionStrings["exceldb"].ConnectionString;
    public void CreateTable(DataTable table)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        string sql = GetCreateTableQuery(table);
        SqlCommand cmd = new(sql, conn);
        cmd.ExecuteNonQuery();
    }

    public void DropExistingDataBase()
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        string sql = "DROP DATABASE IF EXISTS exceldb";
        SqlCommand cmd = new(sql, conn);
        cmd.ExecuteNonQuery();
    }

    public void CreateNewDatabase()
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        string sql = "CREATE DATABASE exceldb";
        SqlCommand cmd = new(sql, conn);
        cmd.ExecuteNonQuery();
    }

    public void DisplayTable()
    {
        DataTable table = new();
        using var conn = new SqlConnection(connectionString);
        string sql = "SELECT * FROM exceldb.dbo.excel;";
        SqlCommand cmd = new(sql, conn);
        conn.Open();
        using (var reader = cmd.ExecuteReader())
        {
            table.Load(reader);
        }

        DisplayData(table);
    }

    public void InsertIntoTable(DataTable table)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        using SqlBulkCopy bulkCopy = new(conn);
        bulkCopy.DestinationTableName = "exceldb.dbo.excel";

        try
        {
            bulkCopy.WriteToServer(table);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void DisplayData(System.Data.DataTable table)
    {
        foreach (System.Data.DataRow row in table.Rows)
        {
            foreach (System.Data.DataColumn col in table.Columns)
            {
                Console.WriteLine("{0} = {1}", col.ColumnName, row[col]);
            }
            Console.WriteLine("============================");
        }
    }

    private static string GetCreateTableQuery(DataTable table)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append("CREATE TABLE exceldb.dbo.excel (");
        for (int i = 0; i < table.Columns.Count; i++)
        {
            stringBuilder.Append("\n [" + table.Columns[i].ColumnName + "] ");
            string columnType = table.Columns[i].DataType.ToString();
            switch (columnType)
            {
                case "System.Int32":
                    stringBuilder.Append(" int ");
                    break;
                default:
                    stringBuilder.Append(string.Format(" nvarchar({0}) ", table.Columns[i].MaxLength == -1 ? "max" : table.Columns[i].MaxLength.ToString()));
                    break;
            }
            if (table.Columns[i].AutoIncrement)
            {
                stringBuilder.Append(" IDENTITY(" + table.Columns[i].AutoIncrementSeed.ToString() + "," + table.Columns[i].AutoIncrementStep.ToString() + ") ");

            }
            if (!table.Columns[i].AllowDBNull)
            {
                stringBuilder.Append(" NOT NULL ");
            }
            stringBuilder.Append(',');
        }

        return stringBuilder.ToString()[..(stringBuilder.Length - 1)] + "\n)";
    }
}
