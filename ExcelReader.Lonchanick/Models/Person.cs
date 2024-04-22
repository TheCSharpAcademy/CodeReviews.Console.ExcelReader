namespace exelReader2._0.Models;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SecondName { get; set; }
    public string City { get; set; }
    public string Phone { get; set; }
    public string Mail { get; set; }

    public void PrintPerson()
    {
        Console.WriteLine($"{Name} - {SecondName} - {City} - {Phone} - {Mail}");
    }
    public static void PrintPersons(IEnumerable<Person> Persons)
    {
        foreach (var person in Persons)
            person.PrintPerson();
    }
}
