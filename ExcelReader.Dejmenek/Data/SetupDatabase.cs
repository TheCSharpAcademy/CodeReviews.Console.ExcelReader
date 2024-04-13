using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace ExcelReader.Dejmenek.Data;
public class SetupDatabase : ISetupDatabase
{
    private readonly string _serverConnectionString;
    private readonly string _dbConnectionString;

    public SetupDatabase(IConfiguration configuration)
    {
        _serverConnectionString = configuration.GetConnectionString("ServerConnection");
        _dbConnectionString = configuration.GetConnectionString("InventoryDb");
    }

    public void Run()
    {
        DeleteInventoryDatabase();
        CreateInventoryDatabase();
        CreateItemsTable();
    }

    public void CreateInventoryDatabase()
    {
        AnsiConsole.MarkupLine("Creating inventory database");

        using (var connection = new SqlConnection(_serverConnectionString))
        {
            string sql = @"
                CREATE DATABASE Inventory;
            ";

            connection.Execute(sql);
        }
    }

    public void DeleteInventoryDatabase()
    {
        AnsiConsole.MarkupLine("Deleting inventory database");

        using (var connection = new SqlConnection(_serverConnectionString))
        {
            string sql = @"
                DROP DATABASE IF EXISTS Inventory;
            ";

            connection.Execute(sql);
        }
    }

    public void CreateItemsTable()
    {
        AnsiConsole.MarkupLine("Creating items table");

        using (var connection = new SqlConnection(_dbConnectionString))
        {
            string sql = @"
                CREATE TABLE Items (
                    Id int NOT NULL PRIMARY KEY,
                    Name varchar(max) NOT NULL,
                    Quantity int NOT NULL,
                    UnitPrice decimal(5, 2) NOT NULL
                );
            ";

            connection.Execute(sql);
        }
    }
}
