using System.Text;
using ExcelReader.Models;
using OfficeOpenXml;

namespace ExcelReader.Services;

/// <summary>
/// Reads a CSV file with EPPlus.
/// </summary>
public class CsvDataFileReader : DataFileReader, ICsvDataFileReader
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

        using var package = new ExcelPackage();

        // Import CSV file into in memory Excel doc.
        var worksheet = package.Workbook.Worksheets.Add(fileInfo.Name);
        worksheet.Cells["A1"].LoadFromText(fileInfo, new ExcelTextFormat
        {
            Delimiter = ',',
            Encoding = new UTF8Encoding()
        });

        dataFile.DataSheets = GenerateDataSheets(package);

        return dataFile;
    }

    #endregion
}
