using ExcelReader.Models;

namespace ExcelReader.Services;

/// <summary>
/// Definition of the CSV Data File Reader that must be implemented.
/// </summary>
public interface ICsvDataFileReader
{
    DataFile ReadDataFile(FileInfo fileInfo);
}