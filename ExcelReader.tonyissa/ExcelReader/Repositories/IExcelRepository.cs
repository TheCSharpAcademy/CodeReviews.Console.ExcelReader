namespace ExcelReader.Repositories;

interface IExcelRepository<TEntity>
    where TEntity : class
{
    Task AddNewEntry(TEntity entry);
    Task<TEntity[]> GetAllEntries();
}