using ExcelReader.Models;

namespace ExcelReader.Repositories.Interfaces;

internal interface IPersonRepository
{
    void AddPerson(Person person);
    void DropTable();
    List<Person> GetAll();
}
