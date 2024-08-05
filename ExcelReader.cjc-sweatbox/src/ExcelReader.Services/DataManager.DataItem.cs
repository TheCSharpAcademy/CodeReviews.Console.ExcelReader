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
    /// Creates the Cell in the Repository and returns the ID of the created entity.
    /// </summary>
    /// <param name="cell">The Cell to create in the Repository.</param>
    /// <returns>The ID of the created entity.</returns>
    public async Task<int> CreateAsync(DataItem cell)
    {
        var entity = DataItemEntity.MapFrom(cell);

        return await _unitOfWork.DataItems.AddAndGetIdAsync(entity);
    }

    public async Task<IReadOnlyList<DataItem>> GetDataItemsAsync()
    {
        var output = await _unitOfWork.DataItems.GetAsync();
        return output.Select(DataItemEntity.MapTo).ToList();
    }

    public async Task<DataItem?> GetDataItemAsync(int id)
    {
        var output = await _unitOfWork.DataItems.GetAsync(id);
        return output is null ? null : DataItemEntity.MapTo(output);
    }

    public async Task<IReadOnlyList<DataItem>> GetDataItemsByRowIdAsync(int rowId)
    {
        var output = await _unitOfWork.DataItems.GetByDataSheetRowIdAsync(rowId);
        return output.Select(DataItemEntity.MapTo).ToList();
    }

    public async Task<bool> UpdateAsync(DataItem cell)
    {
        var entity = DataItemEntity.MapFrom(cell);

        var result = await _unitOfWork.DataItems.UpdateAsync(entity);

        return result > 0;
    }

    public async Task<bool> DeleteAsync(DataItem cell)
    {
        var entity = DataItemEntity.MapFrom(cell);

        var result = await _unitOfWork.DataItems.DeleteAsync(entity);

        return result > 0;
    }

    #endregion
}
