using Spectre.Console;
using ExcelReader.kwm0304.Views;
using ExcelReader.kwm0304.Data;
using ExcelReader.kwm0304.Services;

public class AppSession : IDisposable
{
  private readonly DatabaseAccess _dbAccess;
  private readonly CsvParserService _service;

  public AppSession(DatabaseAccess dbAccess)
  {
    _dbAccess = dbAccess ?? throw new ArgumentNullException(nameof(dbAccess));
    _service = new CsvParserService();
  }

  public void OnStart()
  {
    _dbAccess.CreateDatabase();
    _dbAccess.DropAllTables();
    try
    {
      AnsiConsole.WriteLine("To use one of the example csv files, right click on the csv you want to use and select 'Copy Path'");
      string inputPath = UserInput.GetUserPath().Trim('"');
      if (!File.Exists(inputPath))
      {
        throw new FileNotFoundException("The specified file does not exist.", inputPath);
      }

      FileInfo filePath = new(inputPath);
      FileInfo newInfo;
      if (!filePath.FullName.EndsWith("xlsx"))
      {
        string xlsxFilePath = Path.ChangeExtension(filePath.FullName, ".xlsx");

        _service.ConvertCsvToXlsx(filePath.FullName, xlsxFilePath);

        newInfo = new FileInfo(xlsxFilePath);
      }
      else
      {
        newInfo = filePath;
      }

      var response = _service.ParseCsvFromWorkbook(newInfo, 1, 2);

      if (response.RowValues.Count == 0)
      {
        AnsiConsole.WriteLine("No data was parsed from the file.");
        return;
      }

      var excelTable = new GenericTable(Path.GetFileNameWithoutExtension(filePath.Name), response.ColumnNames, response.RowValues);
      excelTable.Show();
      _dbAccess.SaveToDatabase(response);

      AnsiConsole.WriteLine("Data has been successfully saved to the database.");
    }
    catch (Exception ex)
    {
      AnsiConsole.WriteException(ex);
    }
    finally
    {
      AnsiConsole.WriteLine("Press Enter to exit...");
      Console.ReadLine();
    }
  }

  public void Dispose()
  {
    (_dbAccess as IDisposable)?.Dispose();
    (_service as IDisposable)?.Dispose();
  }
}