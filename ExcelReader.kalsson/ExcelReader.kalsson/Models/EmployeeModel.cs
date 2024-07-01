using System.ComponentModel.DataAnnotations;

namespace ExcelReader.kalsson.Models;

public class EmployeeModel
{
    [Key]
    public string EEID { get; set; }
    public string FullName { get; set; }
    public string JobTitle { get; set; }
    public string Department { get; set; }
    public string BusinessUnit { get; set; }
    public string Gender { get; set; }
    public string Ethnicity { get; set; }
    public int Age { get; set; }
    public DateTime HireDate { get; set; }
    public decimal AnnualSalary { get; set; }
    public decimal BonusPercentage { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public DateTime? ExitDate { get; set; }
}