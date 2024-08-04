using ExcelReader.kwm0304.Data;
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
    FileInfo filePath = UserInput.GetUserPath();
    int headerRow = UserInput.GetRowOptions("What line are your column names on?");
    int dataStart = UserInput.GetRowOptions("What line does your row data start on?");
    var response = _service.ParseCsv(filePath, headerRow, dataStart);
    List<string> colNames = response.ColumnNames;
    var excelTable = new GenericTable(response.Header, colNames, response.RowValues);
    excelTable.Show();
    _dbAccess.SaveToDatabase(response);
    AnsiConsole.WriteLine("Press any key to exit...");
    Console.ReadKey(true);
  }
}