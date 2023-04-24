namespace ExcelReader.Model;

public class Aliment
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? DailyDosage { get; set; } = "Unknown";
    public string? Calories { get; set; } = "Unknown";
    public string? Proteines { get; set; } = "Unknown";
    public string? DailyCalories { get; set; } = "Unknown";
    public string? DailyProteines { get; set; } = "Unknown";
}