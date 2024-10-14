using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Repositories;

public interface IExcelRepository<TEntity, TContext>
    where TContext : DbContext
    where TEntity : class
{
    Task CommitEntryAsync(TEntity entry);
    Task<TEntity[]> RetrieveEntriesAsync();
}