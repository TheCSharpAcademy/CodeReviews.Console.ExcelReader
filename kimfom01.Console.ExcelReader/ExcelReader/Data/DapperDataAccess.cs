using System.Data;
using Dapper;
using ExcelReader.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ExcelReader.Data;

public class DapperDataAccess : IDataAccess
{
    private readonly IConfiguration _config;
    private string _connectionString;

    public DapperDataAccess(IConfiguration config)
    {
        _config = config;
        InitializeConnectionString();
    }

    private void InitializeConnectionString()
    {
        _connectionString = _config.GetConnectionString("ExcelDb");
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        using IDbConnection connection = new SqlConnection(_connectionString);

        var products = await connection.QueryAsync<Product>("dbo.Products_Select");

        return products.ToList();
    }

    public async Task AddProducts(List<Product> products)
    {
        using IDbConnection connection = new SqlConnection(_connectionString);

        await connection.ExecuteAsync("dbo.Products_Insert @ProductId, @ProductName, @Supplier, @ProductCost", products);
    }
}
