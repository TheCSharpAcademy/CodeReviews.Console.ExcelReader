using System.Data.SQLite;
using Dapper;
using ExcelReader.Configurations;
using ExcelReader.Data.Entities;
using Microsoft.Extensions.Options;

namespace ExcelReader.Data.Repositories;

/// <summary>
/// Specific database methods required for the Data Item entity.
/// </summary>
public class DataItemRepository : SqliteEntityRepository<DataItemEntity>, IDataItemRepository
{
    #region Constructors

    public DataItemRepository(IOptions<ApplicationOptions> options) : base(options)
    {
    }

    #endregion
    #region Methods

    public async Task<IEnumerable<DataItemEntity>> GetByDataSheetRowIdAsync(int dataSheetRowId)
    {
        var table = GetTableName();

        string query = $"SELECT * FROM {table} WHERE DataSheetRowId = '{dataSheetRowId}';";

        using var connection = new SQLiteConnection(ConnectionString);

        return await connection.QueryAsync<DataItemEntity>(query);
    }

    #endregion
}
