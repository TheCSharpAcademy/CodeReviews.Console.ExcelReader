using Microsoft.Data.SqlClient;
using OfficeOpenXml;

class Program
{
    static string connectionString = "Data Source=(localdb)\\LocalDBDemo;Initial Catalog=ExcelData;Integrated Security=True;Connect Timeout=30;Encrypt=False";
    public static void Main(string[] args)
    {
        CreateTable();
        SeedTable();
    }

    private static ExcelWorksheet GetWorksheet()
    {
        Console.Clear();
        Console.WriteLine("Getting excel file...");
        string path = @""; //Type the path of the excel file to read
        FileInfo fileInfo = new FileInfo(path);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ExcelPackage package = new ExcelPackage(fileInfo);

        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

        return worksheet;
    }

    private static void CreateTable()
    {
        ExcelWorksheet worksheet = GetWorksheet();
        int columnCount = worksheet.Dimension.Columns;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            bool loading = true;
            while (loading)
            {
                Console.Clear();
                Console.WriteLine("Creating table...");
                try
                {
                    string columnsToAdd = "";
                    List<string> columns = new();
                    for (int k = 0; k < columnCount; k++)
                    {
                        var addedColumnToList = worksheet.Cells[1, k + 1];
                        columns.Add(addedColumnToList.Value.ToString());
                    }
                    foreach (var column in columns)
                    {
                        columnsToAdd += $", {column} TEXT";
                    }

                    using (SqlCommand command = new SqlCommand(@"IF OBJECT_ID('ExcelData') IS NOT NULL 
DROP TABLE ExcelData", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(@$"IF OBJECT_ID('ExcelData') IS NULL 
CREATE TABLE ExcelData(
Id INT IDENTITY(1,1) PRIMARY KEY{columnsToAdd})", connection))
                    {
                        command.ExecuteNonQuery();
                        loading = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                }
            }
            connection.Close();
        }
    }

    private static void SeedTable()
    {
        Console.Clear();
        Console.WriteLine("Reading from excel...");

        ExcelWorksheet worksheet = GetWorksheet();
        int columnCount = worksheet.Dimension.Columns;
        int rowCount = worksheet.Dimension.Rows;
        List<string> columns = new();
        for (int k = 0; k < columnCount; k++)
        {
            var addedColumnToList = worksheet.Cells[1, k + 1];
            columns.Add(addedColumnToList.Value.ToString());
        }

        string columnsString = "";

        foreach (var column in columns)
        {
            if (columnsString == "")
            {
                columnsString += column;
            }
            else
            {
                columnsString += $", {column}";
            }
        }

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            Console.Clear();
            Console.WriteLine("Seeding table...");
            for (int l = 2; l < rowCount + 1; l++)
            {
                string rowString = "";
                for (int j = 0; j < columnCount; j++)
                {
                    var rowToAdd = worksheet.Cells[l, j + 1].Value;
                    if (rowString == "")
                    {
                        rowString += $"'{rowToAdd}'";
                    }
                    else
                    {
                        rowString += $", '{rowToAdd}'";
                    }
                }
                using (SqlCommand command = new SqlCommand($"INSERT INTO ExcelData({columnsString}) VALUES({rowString})", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
            Console.Clear();
            Console.WriteLine("Data from excel added with success!");
        }
    }
}
