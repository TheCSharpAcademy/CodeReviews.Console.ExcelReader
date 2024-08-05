using System.Data.SQLite;
using Dapper;
using ExcelReader.Configurations;
using ExcelReader.Data.Entities;
using Microsoft.Extensions.Options;

namespace ExcelReader.Data.Repositories;

/// <summary>
/// Specific database methods required for the Data Sheet Row entity.
/// </summary>
public class DataSheetRowRepository : SqliteEntityRepository<DataSheetRowEntity>, IDataSheetRowRepository
{
    #region Constructors

    public DataSheetRowRepository(IOptions<ApplicationOptions> options) : base(options)
    {
    }

    #endregion
    #region Methods
    public async Task<IEnumerable<DataSheetRowEntity>> GetByDataSheetIdAsync(int dataSheetId)
    {
        var table = GetTableName();

        string query = $"SELECT * FROM {table} WHERE DataSheetId = '{dataSheetId}';";

        using var connection = new SQLiteConnection(ConnectionString);

        return await connection.QueryAsync<DataSheetRowEntity>(query);
    }

    #endregion
}
