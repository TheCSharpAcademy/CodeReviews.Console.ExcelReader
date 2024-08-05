using ExcelReader.Models;
using Microsoft.Extensions.Logging;

namespace ExcelReader.Services;

/// <summary>
/// Processes a file and routes to the correct DataFileReader implementation.
/// </summary>
public class DataFileProcessor : IDataFileProcessor
{
    #region Fields

    private readonly ILogger<DataFileProcessor> _logger;
    private readonly ICsvDataFileReader _csvDataFileReader;
    private readonly IExcelDataFileReader _excelDataFileReader;

    #endregion
    #region Constructors

    public DataFileProcessor(ILogger<DataFileProcessor> logger, ICsvDataFileReader csvDataFileReader, IExcelDataFileReader excelDataFileReader)
    {
        _logger = logger;
        _csvDataFileReader = csvDataFileReader;
        _excelDataFileReader = excelDataFileReader;
    }

    #endregion
    #region Methods

    public DataFile ProcessFile(FileInfo fileInfo)
    {
        ArgumentNullException.ThrowIfNull(fileInfo, nameof(fileInfo));

        return fileInfo.Extension.ToLower() switch
        {
            ".csv" => _csvDataFileReader.ReadDataFile(fileInfo),
            ".xlsx" => _excelDataFileReader.ReadDataFile(fileInfo),
            _ => throw new InvalidOperationException($"Unsupported file type: {fileInfo.Extension}"),
        };
    }

    #endregion
}
