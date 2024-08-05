using ExcelReader.Data.Entities;

namespace ExcelReader.Data.Repositories;

/// <summary>
/// Definition of the Data File Repository that must be implemented.
/// </summary>
public interface IDataFileRepository : IEntityRepository<DataFileEntity>
{
}
