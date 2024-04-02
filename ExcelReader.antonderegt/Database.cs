using Microsoft.EntityFrameworkCore;

namespace ExcelReader;

public class Database
{
    private readonly ExcelReaderContext _context;

    public Database()
    {
        try
        {
            _context = new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating database context: {ex.Message}");
            Environment.Exit(0);
        }
    }

    public void Initialize()
    {
        try
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error ensuring database creation: {ex.Message}");
        }
    }

    public async Task AddToDatabase(List<Number> numbers)
    {
        try
        {
            await _context.Numbers.AddRangeAsync(numbers);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding numbers to database: {ex.Message}");
        }
    }

    public async Task ReadFromDatabase()
    {
        try
        {
            List<Number> numbers = await _context.Numbers.ToListAsync();
            foreach (Number number in numbers)
            {
                Console.WriteLine(number.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading numbers from database: {ex.Message}");
        }
    }
}