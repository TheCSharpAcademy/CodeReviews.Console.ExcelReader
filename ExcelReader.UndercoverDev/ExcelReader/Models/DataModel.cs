namespace ExcelReader.Models;
public class DataModel
{
    public int Id { get; set; }
    public required string Date { get; set; }
    public required string League { get; set; }
    public required string Home { get; set; }
    public required string Away { get; set; }
    public required string HomeProbability { get; set; }
    public required string AwayProbability { get; set; }
    public required string OverTwoGoals { get; set; }
}