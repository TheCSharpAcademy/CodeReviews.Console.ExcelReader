using ExcelReader.Speedierone.Model;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Speedierone
{
    internal class DataInsert
    {
        public static void DeleteExistingRecords()
        {
            using (var dbContext = new OrdersContext())
            {
                dbContext.Database.EnsureDeleted();

                dbContext.Database.EnsureCreated();

                dbContext.Database.ExecuteSqlRaw("DELETE FROM Orders");
            }
        }

        public static void InsertIntoSqlServer(List<Orders> orders)
        {
            using (var dbContext = new OrdersContext())
            {
                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();
            }
        }
    }
}
