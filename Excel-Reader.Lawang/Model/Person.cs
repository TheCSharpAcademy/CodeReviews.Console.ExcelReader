using System;

namespace Excel_Reader.Lawang.Model;

public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; }  = null!;
    public string Gender { get; set; } = null!;
    public string Country { get; set; } = null!;
    public int Age { get; set; }
    public string Date { get; set; } = null!;
}
