using ExcelReader.Data.Entities;

namespace ExcelReader.Data.Repositories;

/// <summary>
/// Definition of the Data Sheet Repository that must be implemented.
/// </summary>
public interface IDataSheetRepository : IEntityRepository<DataSheetEntity>
{
    Task<IEnumerable<DataSheetEntity>> GetByDataFileIdAsync(int dataFileId);
}
