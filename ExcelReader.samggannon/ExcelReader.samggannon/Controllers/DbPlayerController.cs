using ExcelReader.samggannon.Data;
using ExcelReader.samggannon.Services;
using ExcelReader.samggannon.UI;

namespace ExcelReader.samggannon.Controllers;

internal class DbPlayerController
{
    private readonly IPlayerService _playerService;

    public DbPlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    public async Task EnsureDelete()
    {
       
        Console.WriteLine("Deleting exisiting database...");
        bool isDeleted = await _playerService.DeletePlayerDataDb();
        if (isDeleted)
        {
            Console.WriteLine("Database was deleted...");
        }
        else
        {
            Console.WriteLine("Database didn't exist.");
        }
    }

    public async Task EnsureCreate()
    {
        Console.WriteLine("Creating database");
        bool isCreated = await _playerService.CreatePlayerDataDb();

        if (!isCreated)
        {
            Console.WriteLine("Unable to create the database");
            return;
        }

        Console.WriteLine("Database successfully created");
    }
}
