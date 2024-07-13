namespace ExcelReader.Data;

public class DatabaseCreateOrDelete
{
    private readonly DataContext context = new();

    public void CreateOrDeleteDb()
    {
        Console.WriteLine("Deleting database...\n");
        var databaseDelete = context.Database.EnsureDeleted();
        if (!databaseDelete) Console.WriteLine("Database could not be deleted\n");
        var databaseCreate = context.Database.EnsureCreated();
        Console.WriteLine("Database created\n.");
        if (!databaseCreate) Console.WriteLine("Database could not be created\n");
    }
}