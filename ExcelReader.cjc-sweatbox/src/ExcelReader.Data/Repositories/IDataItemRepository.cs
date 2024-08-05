using ExcelReader.Data.Entities;

namespace ExcelReader.Data.Repositories;

/// <summary>
/// Definition of the Data Item Repository that must be implemented.
/// </summary>
public interface IDataItemRepository : IEntityRepository<DataItemEntity>
{
    Task<IEnumerable<DataItemEntity>> GetByDataSheetRowIdAsync(int dataSheetRowId);
}
