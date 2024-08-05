using ExcelReader.Data.Entities;

namespace ExcelReader.Data.Repositories;

/// <summary>
/// Definition of the Data Sheet Row Repository that must be implemented.
/// </summary>
public interface IDataSheetRowRepository : IEntityRepository<DataSheetRowEntity>
{
    Task<IEnumerable<DataSheetRowEntity>> GetByDataSheetIdAsync(int dataSheetId);
}
