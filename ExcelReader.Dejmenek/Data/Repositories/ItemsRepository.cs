using Dapper;
using ExcelReader.Dejmenek.Data.Interfaces;
using ExcelReader.Dejmenek.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ExcelReader.Dejmenek.Data.Repositories;
public class ItemsRepository : IItemsRepository
{
    private readonly string _inventoryConnectionString;

    public ItemsRepository(IConfiguration configuration)
    {
        _inventoryConnectionString = configuration.GetConnectionString("InventoryDb");
    }

    public List<Item> GetItems()
    {
        using (var connection = new SqlConnection(_inventoryConnectionString))
        {
            string sql = @"
                SELECT * FROM Items;
            ";

            return connection.Query<Item>(sql).ToList();
        }
    }

    public void CreateItems(List<Item> items)
    {
        using (var connection = new SqlConnection(_inventoryConnectionString))
        {
            string sql = @"
                INSERT INTO Items (Id, Name, Quantity, UnitPrice) VALUES (@Id, @Name, @Quantity, @UnitPrice);
            ";
            foreach (var item in items)
            {
                connection.Execute(sql, new
                {
                    item.Id,
                    item.Name,
                    item.Quantity,
                    item.UnitPrice
                });
            }
        }
    }
}
