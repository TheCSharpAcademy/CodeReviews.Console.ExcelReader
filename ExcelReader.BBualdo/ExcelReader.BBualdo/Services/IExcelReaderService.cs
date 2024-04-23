using ExcelReader.BBualdo.Models;

namespace ExcelReader.BBualdo.Services;

public interface IExcelReaderService
{
  Task<List<Person>> GetFromExcel();
}