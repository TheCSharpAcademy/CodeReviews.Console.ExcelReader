using ExcelReader.samggannon.Data;
using ExcelReader.samggannon.Services;
using ExcelReader.samggannon.UI;

namespace ExcelReader.samggannon.Controllers;

internal class DbPlayerConroller
{
    private readonly IPlayerService _playerService;

    public DbPlayerConroller(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    public async Task EnsureDelete()
    {
       
        ConsoleOutput.InformUser("Deleting exisiting database...");
        bool isDeleted = await _playerService.DeletePlayerDataDb();
        if (isDeleted)
        {
            ConsoleOutput.InformUser("Database was deleted...");
        }
        else
        {
            ConsoleOutput.InformUser("Database didn't exist.");
        }
    }

    public async void EnsureCreate()
    {
        ConsoleOutput.InformUser("Creating and seeding database");
        bool isCreated = await _playerService.CreatePlayerDataDb();

        if (!isCreated)
        {
            Console.WriteLine("Unable to create the database");
            return;
        }

        Console.WriteLine("Database successfully created");
    }
}
