using System.ComponentModel.DataAnnotations;

namespace ExcelReader.frockett.Models;

internal class Employee
{
    public int Id { get; set; }
    [Required] public string EmployeeId { get; set; }
    [Required] public string EmployeeName { get; set; }
    [Required] public string JobTitle { get; set; }
    [Required] public string Department { get; set; }
    [Required] public string BusinessUnit { get; set; }
    [Required] public string Gender { get; set; }
    [Required] public int Age { get; set; }
    [Required] public DateOnly HireDate { get; set; }
    [Required] public int AnnualSalary { get; set; }
    [Required] public int BonusPercent { get; set; }
    [Required] public string Country {  get; set; }
    [Required] public string City { get; set; }
    public DateOnly? ExitDate { get; set; }
}
