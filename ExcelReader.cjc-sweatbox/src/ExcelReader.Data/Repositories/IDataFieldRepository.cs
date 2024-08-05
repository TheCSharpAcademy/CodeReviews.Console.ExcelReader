using ExcelReader.Data.Entities;

namespace ExcelReader.Data.Repositories;

/// <summary>
/// Definition of the Data Field Repository that must be implemented.
/// </summary>
public interface IDataFieldRepository : IEntityRepository<DataFieldEntity>
{
    Task<IEnumerable<DataFieldEntity>> GetByDataSheetIdAsync(int dataSheetId);
}
