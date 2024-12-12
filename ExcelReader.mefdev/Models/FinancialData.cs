namespace ExcelReader.mefdev.Models;

public class FinancialData
{
    public int Id { get; set; }
    public string Account { get; set; }
    public string BusinessUnit { get; set; }
    public string Currency { get; set; }
    public int Year { get; set; }
    public string Scenario { get; set; }
    public decimal Jan { get; set; }
    public decimal Feb { get; set; }
    public decimal Mar { get; set; }
    public decimal Apr { get; set; }
    public decimal May { get; set; }
    public decimal Jun { get; set; }
    public decimal Jul { get; set; }
    public decimal Aug { get; set; }
    public decimal Sep { get; set; }
    public decimal Oct { get; set; }
    public decimal Nov { get; set; }
    public decimal Dec { get; set; }
}