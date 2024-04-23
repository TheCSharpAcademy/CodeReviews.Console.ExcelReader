using ExcelReader.BBualdo.Models;
using OfficeOpenXml;

namespace ExcelReader.BBualdo.Services;

public class ExcelReaderService : IExcelReaderService
{
  public FileInfo File { get; private set; }
  public int StartColumn { get; private set; }
  public int StartRow { get; private set; }

  public ExcelReaderService()
  {
    File = new FileInfo(System.Configuration.ConfigurationManager.AppSettings.Get("FilePath")!);
    StartColumn = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("StartCol")!);
    StartRow = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("StartRow")!);
  }

  public async Task<List<Person>> GetFromExcel()
  {
    List<Person> output = [];
    using ExcelPackage package = new ExcelPackage(File);
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

    if (!File.Exists)
    {
      Console.WriteLine("File not found.");
      return output;
    }
    else
    {
      await package.LoadAsync(File);
      ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

      while (string.IsNullOrWhiteSpace(worksheet.Cells[StartRow, StartColumn].Value?.ToString()) == false)
      {
        Person person = new();
        person.FirstName = worksheet.Cells[StartRow, StartColumn].Value.ToString();
        person.LastName = worksheet.Cells[StartRow, StartColumn + 1].Value.ToString();
        person.Age = int.Parse(worksheet.Cells[StartRow, StartColumn + 2].Value.ToString()!);
        person.Country = worksheet.Cells[StartRow, StartColumn + 3].Value.ToString();
        output.Add(person);
        StartRow++;
      }

      return output;
    }
  }
}