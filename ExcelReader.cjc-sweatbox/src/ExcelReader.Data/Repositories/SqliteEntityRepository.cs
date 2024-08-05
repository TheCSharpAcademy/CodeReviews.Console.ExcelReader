using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Text;
using Dapper;
using ExcelReader.Configurations;
using Microsoft.Extensions.Options;
using Database = SQLite;

namespace ExcelReader.Data.Repositories;

/// <summary>
/// Sqlite generic entity repository methods.
/// </summary>
public class SqliteEntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class
{
    #region Fields

    private readonly ApplicationOptions _options;

    #endregion
    #region Constructors

    public SqliteEntityRepository(IOptions<ApplicationOptions> options)
    {
        _options = options.Value;
    }

    #endregion
    #region Properties

    public string ConnectionString => $"Data Source={FileName}";

    private string FileName => Path.ChangeExtension(_options.DatabaseName, _options.DatabaseExtension);

    private string FilePath => Path.GetFullPath(FileName);

    #endregion
    #region Methods - Public

    public async Task<int> AddAsync(TEntity entity)
    {
        string tableName = GetTableName();
        string columns = SqliteEntityRepository<TEntity>.GetColumns(true);
        string properties = GetPropertyNames(true);
        string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties})";

        using var connection = new SQLiteConnection(ConnectionString);
        return await connection.ExecuteAsync(query, entity);
    }

    public async Task<int> AddAndGetIdAsync(TEntity entity)
    {
        string tableName = GetTableName();
        string columns = SqliteEntityRepository<TEntity>.GetColumns(true);
        string properties = GetPropertyNames(true);
        string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties}); SELECT last_insert_rowid();";

        using var connection = new SQLiteConnection(ConnectionString);
        return await connection.ExecuteScalarAsync<int>(query, entity);
    }

    public bool CreateTable()
    {
        var database = new Database.SQLiteConnection(FilePath);
        var result = database.CreateTable<TEntity>();
        return result is Database.CreateTableResult.Created;
    }

    public async Task<int> DeleteAsync(TEntity entity)
    {
        string tableName = GetTableName();
        string keyColumn = GetKeyColumnName()!;
        string keyProperty = GetKeyPropertyName()!;
        string query = $"DELETE FROM {tableName} WHERE {keyColumn} = @{keyProperty}";

        using var connection = new SQLiteConnection(ConnectionString);
        return await connection.ExecuteAsync(query, entity);
    }

    public async Task<IEnumerable<TEntity>> GetAsync()
    {
        string tableName = GetTableName();
        string query = $"SELECT * FROM {tableName}";

        using var connection = new SQLiteConnection(ConnectionString);
        return await connection.QueryAsync<TEntity>(query);
    }

    public async Task<TEntity?> GetAsync(int id)
    {
        string tableName = GetTableName();
        string keyColumn = GetKeyColumnName()!;
        string keyProperty = GetKeyPropertyName()!;
        string query = $"SELECT * FROM {tableName} WHERE {keyColumn} = '{id}'";

        using var connection = new SQLiteConnection(ConnectionString);
        return await connection.QuerySingleOrDefaultAsync<TEntity>(query);
    }

    public string GetTableName()
    {
        var type = typeof(TEntity);
        var tableAttribute = type.GetCustomAttribute<Database.TableAttribute>();
        return tableAttribute is null ? type.Name : tableAttribute.Name;
    }

    public async Task<int> UpdateAsync(TEntity entity)
    {
        string tableName = GetTableName();
        string keyColumn = GetKeyColumnName()!;
        string keyProperty = GetKeyPropertyName()!;

        var query = new StringBuilder();
        query.Append($"UPDATE {tableName} SET ");
        foreach (var property in GetProperties(true))
        {
            var columnAttribute = property.GetCustomAttribute<Database.ColumnAttribute>();
            query.Append($"{columnAttribute!.Name} = @{property.Name},");
        }
        query.Remove(query.Length - 1, 1);

        query.Append($"WHERE {keyColumn} = @{keyProperty}");

        using var connection = new SQLiteConnection(ConnectionString);
        return await connection.ExecuteAsync(query.ToString(), entity);
    }

    #endregion
    #region Methods - Private

    public static string? GetKeyColumnName()
    {
        PropertyInfo[] properties = typeof(TEntity).GetProperties();

        foreach (PropertyInfo property in properties)
        {
            object[] keyAttributes = property.GetCustomAttributes(typeof(Database.PrimaryKeyAttribute), true);

            if (keyAttributes != null && keyAttributes.Length > 0)
            {
                object[] columnAttributes = property.GetCustomAttributes(typeof(Database.ColumnAttribute), true);

                if (columnAttributes != null && columnAttributes.Length > 0)
                {
                    Database.ColumnAttribute columnAttribute = (Database.ColumnAttribute)columnAttributes[0];
                    return columnAttribute.Name;
                }
                else
                {
                    return property.Name;
                }
            }
        }

        return null;
    }

    private static string GetColumns(bool excludeKey = false)
    {
        var type = typeof(TEntity);
        var columns = string.Join(", ", type.GetProperties()
            .Where(p => !excludeKey || !p.IsDefined(typeof(Database.PrimaryKeyAttribute)))
            .Select(p =>
            {
                var columnAttr = p.GetCustomAttribute<Database.ColumnAttribute>();
                return columnAttr != null ? columnAttr.Name : p.Name;
            }));

        return columns;
    }

    protected string GetPropertyNames(bool excludeKey = false)
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute<Database.PrimaryKeyAttribute>() == null);

        var values = string.Join(", ", properties.Select(p =>
        {
            return $"@{p.Name}";
        }));

        return values;
    }

    protected IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute<Database.PrimaryKeyAttribute>() == null);

        return properties;
    }

    protected string? GetKeyPropertyName()
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => p.GetCustomAttribute<Database.PrimaryKeyAttribute>() != null);

        return properties.Any() ? (properties.First().Name) : null;
    }

    #endregion

}
