using ExcelReader.Data.Entities;
using ExcelReader.Models;

namespace ExcelReader.Services;

/// <summary>
/// Partial DataManager class for entity specific data access methods.
/// </summary>
public partial class DataManager : IDataManager
{
    #region Methods

    /// <summary>
    /// Creates the Row in the Repository and returns the ID of the created entity.
    /// </summary>
    /// <param name="row">The Row to create in the Repository.</param>
    /// <returns>The ID of the created entity.</returns>
    public async Task<int> CreateAsync(DataSheetRow row)
    {
        var entity = DataSheetRowEntity.MapFrom(row);

        return await _unitOfWork.DataSheetRows.AddAndGetIdAsync(entity);
    }

    public async Task<IReadOnlyList<DataSheetRow>> GetDataSheetRowsAsync()
    {
        var output = await _unitOfWork.DataSheetRows.GetAsync();
        return output.Select(DataSheetRowEntity.MapTo).ToList();
    }

    public async Task<DataSheetRow?> GetDataSheetRowAsync(int id)
    {
        var output = await _unitOfWork.DataSheetRows.GetAsync(id);
        return output is null ? null : DataSheetRowEntity.MapTo(output);
    }

    public async Task<IReadOnlyList<DataSheetRow>> GetDataSheetRowsByWorksheetIdAsync(int worksheetId)
    {
        var output = await _unitOfWork.DataSheetRows.GetByDataSheetIdAsync(worksheetId);
        return output.Select(DataSheetRowEntity.MapTo).ToList();
    }
    public async Task<bool> UpdateAsync(DataSheetRow row)
    {
        var entity = DataSheetRowEntity.MapFrom(row);

        var result = await _unitOfWork.DataSheetRows.UpdateAsync(entity);

        return result > 0;
    }

    public async Task<bool> DeleteAsync(DataSheetRow row)
    {
        var entity = DataSheetRowEntity.MapFrom(row);

        var result = await _unitOfWork.DataSheetRows.DeleteAsync(entity);

        return result > 0;
    }

    #endregion
}
