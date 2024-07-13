using System.ComponentModel.DataAnnotations;

namespace ExcelReader.Models;

public class EmployeeDetails
{
    [Key] public int Id { get; set; }
    public string? EmployeeId { get; set; }

    public string? Name { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? DateOfHire { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public int? Salary { get; set; }
}