namespace ExcelReader.Repositories;

interface IExcelRepository<TEntity>
    where TEntity : class
{
    Task CommitEntryAsync(TEntity entry);
    Task<TEntity[]> RetrieveEntriesAsync();
}