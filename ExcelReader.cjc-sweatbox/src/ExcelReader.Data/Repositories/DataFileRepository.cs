using ExcelReader.Configurations;
using ExcelReader.Data.Entities;
using Microsoft.Extensions.Options;

namespace ExcelReader.Data.Repositories;

/// <summary>
/// Specific database methods required for the Data File entity.
/// </summary>
public class DataFileRepository : SqliteEntityRepository<DataFileEntity>, IDataFileRepository
{
    #region Constructors

    public DataFileRepository(IOptions<ApplicationOptions> options) : base(options)
    {
    }

    #endregion
}
