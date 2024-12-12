using ExcelReader.jollejonas.Data;
using ExcelReader.jollejonas.Models;
namespace ExcelReader.jollejonas.Services;

public class UserService
{
    private readonly ExcelReaderContext _context;

    public UserService(ExcelReaderContext context)
    {
        _context = context;
    }

    public void AddUser(User user)
    {
        if (user.ID == "ID")
        {
            return;
        }

        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void GetUsers()
    {
        foreach (var user in _context.Users)
        {
            Console.WriteLine($"ID: {user.ID}, Name: {user.Name}, Age: {user.Age}, Email: {user.Email}, RegistrationDate: {user.RegistrationDate}");
        }
    }

    public void RecreateDatabase()
    {
        bool databaseDeleted = _context.Database.EnsureDeleted();
        Console.WriteLine(!databaseDeleted ? "Database did not exist - no need to delete" : "Database successfully deleted");

        bool databaseCreated = _context.Database.EnsureCreated();
        Console.WriteLine(!databaseCreated ? "Unable to create database" : "Database created");
    }
}
