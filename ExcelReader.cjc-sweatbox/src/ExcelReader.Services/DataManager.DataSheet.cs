using ExcelReader.Data.Entities;
using ExcelReader.Models;

namespace ExcelReader.Services;

/// <summary>
/// Partial DataManager class for entity specific data access methods.
/// </summary>
public partial class DataManager : IDataManager
{
    #region

    /// <summary>
    /// Creates the Worksheet in the Repository and returns the ID of the created entity.
    /// </summary>
    /// <param name="worksheet">The Worksheet to create in the Repository.</param>
    /// <returns>The ID of the created entity.</returns>
    public async Task<int> CreateAsync(DataSheet worksheet)
    {
        var entity = DataSheetEntity.MapFrom(worksheet);

        return await _unitOfWork.DataSheets.AddAndGetIdAsync(entity);
    }

    public async Task<IReadOnlyList<DataSheet>> GetDataSheetsAsync()
    {
        var output = await _unitOfWork.DataSheets.GetAsync();
        return output.Select(DataSheetEntity.MapTo).ToList();
    }

    public async Task<DataSheet?> GetDataSheetAsync(int id)
    {
        var output = await _unitOfWork.DataSheets.GetAsync(id);
        return output is null ? null : DataSheetEntity.MapTo(output);
    }

    public async Task<IReadOnlyList<DataSheet>> GetDataSheetsByWorkbookIdAsync(int workbookId)
    {
        var output = await _unitOfWork.DataSheets.GetByDataFileIdAsync(workbookId);
        return output.Select(DataSheetEntity.MapTo).ToList();
    }

    public async Task<bool> UpdateAsync(DataSheet worksheet)
    {
        var entity = DataSheetEntity.MapFrom(worksheet);

        var result = await _unitOfWork.DataSheets.UpdateAsync(entity);

        return result > 0;
    }

    public async Task<bool> DeleteAsync(DataSheet worksheet)
    {
        var entity = DataSheetEntity.MapFrom(worksheet);

        var result = await _unitOfWork.DataSheets.DeleteAsync(entity);

        return result > 0;
    }

    #endregion
}
