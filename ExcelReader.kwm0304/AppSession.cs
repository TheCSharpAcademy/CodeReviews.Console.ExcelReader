using ExcelReader.kwm0304.Data;
using ExcelReader.kwm0304.Models;
using ExcelReader.kwm0304.Services;
using ExcelReader.kwm0304.Views;
using Spectre.Console;

namespace ExcelReader.kwm0304;

public class AppSession
{
  private readonly DatabaseAccess _dbAccess;
  private readonly CsvParserService _service;
  public AppSession(DatabaseAccess dbAccess)
  {
    _dbAccess = dbAccess;
    _service = new CsvParserService();
  }
  public void OnStart()
  {
    string inputPath = UserInput.GetUserPath().Trim('"');
    FileInfo filePath = new FileInfo(inputPath);
    int headerRow = UserInput.GetRowOptions("What line are your column names on?");
    int dataStart = UserInput.GetRowOptions("What line does your row data start on?");
    Response<Dictionary<string, object>> response;
    Console.WriteLine("FULLNAME: " + filePath.FullName);
    try
    {
      if (filePath.FullName.EndsWith("csv"))
      {
        response = _service.ParseCsv(filePath, headerRow, dataStart);
        List<string> colNames = response.ColumnNames;
        var excelTable = new GenericTable(Path.GetFileNameWithoutExtension(filePath.Name), colNames, response.RowValues);
        excelTable.Show();
      }
      else if (filePath.FullName.EndsWith(".xlsx") || filePath.Extension.ToLower() == ".xls")
      {
        response = _service.ParseCsvFromWorkbook(filePath, headerRow, dataStart);
        List<string> colNames = response.ColumnNames;
        var excelTable = new GenericTable(Path.GetFileNameWithoutExtension(filePath.Name), colNames, response.RowValues);
        excelTable.Show();
      }
      else
      {
        throw new InvalidDataException("Unsupported file type.");
      }
    }
    catch (InvalidDataException ex)
    {
      AnsiConsole.WriteLine(ex.Message);
      return;
    }
    catch (Exception ex)
    {
      AnsiConsole.WriteLine($"An error occurred: {ex.Message}");
      return;
    }

    if (response.RowValues.Count == 0)
    {
      AnsiConsole.WriteLine("No data was parsed from the file.");
      return;
    }

    _dbAccess.SaveToDatabase(response);
    AnsiConsole.WriteLine("Press any key to exit...");
    Console.ReadKey(true);
  }

}