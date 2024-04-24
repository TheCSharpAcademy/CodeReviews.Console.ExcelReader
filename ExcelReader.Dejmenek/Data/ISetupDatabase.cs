namespace ExcelReader.Dejmenek.Data;
public interface ISetupDatabase
{
    void Run();
    void CreateInventoryDatabase();
    void DeleteInventoryDatabase();
    void CreateItemsTable();
}
