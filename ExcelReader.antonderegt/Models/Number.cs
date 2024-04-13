namespace ExcelReader;

public class Number
{
    public int Id { get; set; }
    public int Digits { get; set; }
    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Id: {Id}, Digits: {Digits}, Name: {Name}";
    }
}