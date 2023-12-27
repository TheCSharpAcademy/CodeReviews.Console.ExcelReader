namespace ExcelReader.UgniusFalze.Models;

public class Plane(
    string manufacturer,
    string model,
    string type,
    long maxSpeed,
    int capacity,
    DateTime firstFlightDate)
{
    public string Manufacturer { get;} = manufacturer;
    public string Model { get; } = model;
    public string Type { get;} = type;
    public long MaxSpeed { get;} = maxSpeed;
    public int Capacity { get;} = capacity;
    public DateTime FirstFlightDate { get;} = firstFlightDate;
}