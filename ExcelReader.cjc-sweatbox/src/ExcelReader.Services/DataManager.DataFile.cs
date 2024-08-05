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
    /// Creates the Workbook in the Repository and returns the ID of the created entity.
    /// </summary>
    /// <param name="workbook">The Workbook to create in the Repository.</param>
    /// <returns>The ID of the created entity.</returns>
    public async Task<int> CreateAsync(DataFile workbook)
    {
        var entity = DataFileEntity.MapFrom(workbook);

        return await _unitOfWork.DataFiles.AddAndGetIdAsync(entity);
    }

    public async Task<IReadOnlyList<DataFile>> GetDataFilesAsync()
    {
        var output = await _unitOfWork.DataFiles.GetAsync();
        return output.Select(DataFileEntity.MapTo).ToList();
    }

    public async Task<DataFile?> GetDataFileAsync(int id)
    {
        var output = await _unitOfWork.DataFiles.GetAsync(id);
        return output is null ? null : DataFileEntity.MapTo(output);
    }

    public async Task<bool> UpdateAsync(DataFile workbook)
    {
        var entity = DataFileEntity.MapFrom(workbook);

        var result = await _unitOfWork.DataFiles.UpdateAsync(entity);

        return result > 0;
    }

    public async Task<bool> DeleteAsync(DataFile workbook)
    {
        var entity = DataFileEntity.MapFrom(workbook);

        var result = await _unitOfWork.DataFiles.DeleteAsync(entity);

        return result > 0;
    }

    #endregion
}
