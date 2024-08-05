using ExcelReader.Models;
using OfficeOpenXml;

namespace ExcelReader.Services;

/// <summary>
/// Definition of the generic Data File Reader that must be implemented.
/// </summary>
public interface IDataFileReader
{
    List<DataField> GenerateDataFields(ExcelWorksheet worksheet);
    List<DataSheetRow> GenerateDataRows(ExcelWorksheet worksheet);
    List<DataSheet> GenerateDataSheets(ExcelPackage package);
}