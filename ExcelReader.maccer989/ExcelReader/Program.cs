using DataAccessLibrary;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ExcelReader
{
    class Program
    {
        static SqliteCrud sql = new SqliteCrud(GetConnectionString());
        static async Task Main(string[] args)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var file = new FileInfo(@"C:\CSharpAcademy\ExcelReader\ExcelReader\ExcelReader.xlsx");
            List<PersonModel> peopleFromExcel = await LoadExcelFile(file);
            await Console.Out.WriteLineAsync("Reading from Excel...");
            Thread.Sleep(1000);
            CreateNewSession(sql, peopleFromExcel);
            await Console.Out.WriteLineAsync("Write from database to console...");
            Thread.Sleep(1000);
            ReadAllSessions(sql);
        }

        static string GetConnectionString(string connectionStringName = "Default")
        {
            string output = "";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            output = config.GetConnectionString(connectionStringName);
            return output;
        }

        private static async Task<List<PersonModel>> LoadExcelFile(FileInfo file)
        {
            List<PersonModel> output = new();
            using var package = new ExcelPackage(file);
            await package.LoadAsync(file);
            var ws = package.Workbook.Worksheets[0];
            int row = 3;
            int col = 1;

            while (string.IsNullOrWhiteSpace(ws.Cells[row, col].Value?.ToString()) == false)
            {
                PersonModel p = new();
                p.Id = int.Parse(ws.Cells[row, col].Value.ToString());
                p.FirstName = ws.Cells[row, col + 1].Value.ToString();
                p.LastName = ws.Cells[row, col + 2].Value.ToString();
                output.Add(p);
                row += 1;                
            }
            return output;
        }

        static void CreateNewSession(SqliteCrud sql, List<PersonModel> person)
        {
            sql.CreateContact(person);
        }

        static void ReadAllSessions(SqliteCrud sql)
        {
            Console.Clear();
            var rows = sql.GetAllSessions();
            TableVisualisation.ShowTable(rows);
        }
    }
}
