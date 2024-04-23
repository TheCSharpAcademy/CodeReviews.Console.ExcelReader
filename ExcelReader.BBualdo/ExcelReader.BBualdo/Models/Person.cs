using System.ComponentModel.DataAnnotations;

namespace ExcelReader.BBualdo.Models;

public class Person
{
  [Key]
  public int Id { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public int Age { get; set; }
  public string? Country { get; set; }
}