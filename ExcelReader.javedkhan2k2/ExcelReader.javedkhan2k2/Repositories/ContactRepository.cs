using ExcelReader.Data;
using ExcelReader.Entities;
using ExcelReader.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Repositories;

public class ContactRepository : IContactRepository
{
    private ContactsDbContext _context;
    public ContactRepository(ContactsDbContext context)
    {
        _context = context;
    }

    public void AddBulkContacts(List<Contact> contacts)
    {
        _context.Contacts.AddRange(contacts);
        _context.SaveChanges();
    }

    public List<Contact> GetAllContacts() => _context.Contacts.ToList();
    
}