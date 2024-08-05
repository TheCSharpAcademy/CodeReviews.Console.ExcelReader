using ExcelReader.Models;

namespace ExcelReader.Services;

/// <summary>
/// Definition of the Data Manager that must be implemented.
/// </summary>
public interface IDataManager
{
    Task<int> CreateAsync(DataField column);
    Task<int> CreateAsync(DataFile workbook);
    Task<int> CreateAsync(DataItem cell);
    Task<int> CreateAsync(DataSheet worksheet);
    Task<int> CreateAsync(DataSheetRow row);
    Task<bool> DeleteAsync(DataField column);
    Task<bool> DeleteAsync(DataFile workbook);
    Task<bool> DeleteAsync(DataItem cell);
    Task<bool> DeleteAsync(DataSheet worksheet);
    Task<bool> DeleteAsync(DataSheetRow row);
    Task<DataField?> GetDataFieldAsync(int id);
    Task<DataFile?> GetDataFileAsync(int id);
    Task<DataItem?> GetDataItemAsync(int id);
    Task<DataSheet?> GetDataSheetAsync(int id);
    Task<DataSheetRow?> GetDataSheetRowAsync(int id);
    Task<IReadOnlyList<DataField>> GetDataFieldsAsync();
    Task<IReadOnlyList<DataField>> GetDataFieldsByWorksheetIdAsync(int worksheetId);
    Task<IReadOnlyList<DataFile>> GetDataFilesAsync();
    Task<IReadOnlyList<DataItem>> GetDataItemsAsync();
    Task<IReadOnlyList<DataItem>> GetDataItemsByRowIdAsync(int rowId);
    Task<IReadOnlyList<DataSheet>> GetDataSheetsAsync();
    Task<IReadOnlyList<DataSheet>> GetDataSheetsByWorkbookIdAsync(int workbookId);
    Task<IReadOnlyList<DataSheetRow>> GetDataSheetRowsAsync();
    Task<IReadOnlyList<DataSheetRow>> GetDataSheetRowsByWorksheetIdAsync(int worksheetId);
    bool ResetDatabase();
    Task<bool> UpdateAsync(DataField column);
    Task<bool> UpdateAsync(DataFile workbook);
    Task<bool> UpdateAsync(DataItem cell);
    Task<bool> UpdateAsync(DataSheet worksheet);
    Task<bool> UpdateAsync(DataSheetRow row);
}