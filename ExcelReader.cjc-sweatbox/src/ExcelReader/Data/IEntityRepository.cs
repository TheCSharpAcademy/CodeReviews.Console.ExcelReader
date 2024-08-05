namespace ExcelReader.Data.Repositories;

/// <summary>
/// Definition of the entity repository that must be implemented.
/// </summary>
public interface IEntityRepository<TEntity> where TEntity : class
{
    Task<int> AddAsync(TEntity entity);
    Task<int> AddAndGetIdAsync(TEntity entity);
    bool CreateTable();
    Task<int> DeleteAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAsync();
    Task<TEntity?> GetAsync(int id);
    Task<int> UpdateAsync(TEntity entity);
}
