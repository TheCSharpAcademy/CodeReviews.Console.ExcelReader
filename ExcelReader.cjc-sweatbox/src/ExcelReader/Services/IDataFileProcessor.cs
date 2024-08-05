using ExcelReader.Models;

namespace ExcelReader.Services;

/// <summary>
/// Definition of the Data File Processor that must be implemented.
/// </summary>
public interface IDataFileProcessor
{
    DataFile ProcessFile(FileInfo fileInfo);
}