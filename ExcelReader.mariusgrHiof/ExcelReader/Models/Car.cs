using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelReader.Models;
public class Car
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Make { get; set; } = string.Empty;
    public string? Model { get; set; } = string.Empty;
    public int? HP { get; set; }
    public int? Year { get; set; }

    public override string ToString()
    {
        return $"{Make} {Model} {HP} {Year}";
    }
}

