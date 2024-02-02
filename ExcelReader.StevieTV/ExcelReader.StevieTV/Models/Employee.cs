namespace ExcelReader.StevieTV.Models;

public class Employee
{
    public long Id { get; set; }
    public string EmployeeId { get; set; }
    public string Name { get; set; }
    public string JobTitle { get; set; }
    public string Department { get; set; }
    public string BusinessUnit { get; set; }
    public string Gender { get; set; }
    public string Ethnicity { get; set; }
    public int Age { get; set; }
    public DateOnly HireDate { get; set; }
    public double Salary { get; set; }
    public float BonusPercent { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public DateOnly ExitDate { get; set; }
}