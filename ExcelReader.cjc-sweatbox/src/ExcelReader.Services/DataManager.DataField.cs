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
    /// Creates the Column in the Repository and returns the ID of the created entity.
    /// </summary>
    /// <param name="column">The Column to create in the Repository.</param>
    /// <returns>The ID of the created entity.</returns>
    public async Task<int> CreateAsync(DataField column)
    {
        var entity = DataFieldEntity.MapFrom(column);

        return await _unitOfWork.DataFields.AddAndGetIdAsync(entity);
    }

    public async Task<IReadOnlyList<DataField>> GetDataFieldsAsync()
    {
        var output = await _unitOfWork.DataFields.GetAsync();
        return output.Select(DataFieldEntity.MapTo).ToList();
    }

    public async Task<DataField?> GetDataFieldAsync(int id)
    {
        var output = await _unitOfWork.DataFields.GetAsync(id);
        return output is null ? null : DataFieldEntity.MapTo(output);
    }

    public async Task<IReadOnlyList<DataField>> GetDataFieldsByWorksheetIdAsync(int worksheetId)
    {
        var output = await _unitOfWork.DataFields.GetByDataSheetIdAsync(worksheetId);
        return output.Select(DataFieldEntity.MapTo).ToList();
    }

    public async Task<bool> UpdateAsync(DataField column)
    {
        var entity = DataFieldEntity.MapFrom(column);

        var result = await _unitOfWork.DataFields.UpdateAsync(entity);

        return result > 0;
    }

    public async Task<bool> DeleteAsync(DataField column)
    {
        var entity = DataFieldEntity.MapFrom(column);

        var result = await _unitOfWork.DataFields.DeleteAsync(entity);

        return result > 0;
    }

    #endregion
}
