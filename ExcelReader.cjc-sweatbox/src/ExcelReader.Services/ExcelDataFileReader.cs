using ExcelReader.Models;
using OfficeOpenXml;

namespace ExcelReader.Services;

/// <summary>
/// Reads an Excel file with EPPlus.
/// </summary>
public class ExcelDataFileReader : DataFileReader, IExcelDataFileReader
{
    #region Methods

    public DataFile ReadDataFile(FileInfo fileInfo)
    {
        ArgumentNullException.ThrowIfNull(fileInfo, nameof(fileInfo));

        var dataFile = new DataFile
        {
            Name = Path.GetFileNameWithoutExtension(fileInfo.Name),
            Extension = fileInfo.Extension.ToLower(),
            Size = fileInfo.Length
        };

        using var package = new ExcelPackage(fileInfo);

        dataFile.DataSheets = GenerateDataSheets(package);

        return dataFile;
    }

    #endregion
}
