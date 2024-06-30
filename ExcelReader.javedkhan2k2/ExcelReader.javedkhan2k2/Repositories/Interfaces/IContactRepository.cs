using ExcelReader.Entities;

namespace ExcelReader.Repositories.Interfaces;

public interface IContactRepository
{
    void AddBulkContacts(List<Contact> contacts);
    List<Contact> GetAllContacts();
}