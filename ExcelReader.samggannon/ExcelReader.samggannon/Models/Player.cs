using System.ComponentModel.DataAnnotations;

namespace ExcelReader.samggannon.Models;

internal class Player
{
    [Key]
    public int Id { get; set; }
    public string? Year { get; set; }
    public string? Team { get; set; }
    public string? Name { get; set; }
    public string? Number { get; set; }
    public string? Pos { get; set; }
    public string? Height { get; set; }
    public string? Weight { get; set; }
    public string? Age { get; set; }
    public string? Experience { get; set; }
    public string? College { get; set; }
}
