using ExcelReader.Models;
using ExcelReader.Repositories.Interfaces;

namespace ExcelReader.Repositories;

internal class PersonRepository : IPersonRepository
{
    private readonly PersonContext _context;

    public PersonRepository(PersonContext context)
    {
        _context = context;
    }

    public void AddPerson(Person person)
    {
        _context.Add(person);
        _context.SaveChanges();
    }

    public void DropTable()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _context.SaveChanges();
    }

    public List<Person> GetAll()
    {
        return _context.Persons.ToList();
    }
}
