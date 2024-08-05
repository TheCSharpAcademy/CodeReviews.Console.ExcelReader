using ExcelReader.Models;

namespace ExcelReader.Services;

/// <summary>
/// Definition of the Excel Data File Reader that must be implemented.
/// </summary>
public interface IExcelDataFileReader
{
    DataFile ReadDataFile(FileInfo fileInfo);
}