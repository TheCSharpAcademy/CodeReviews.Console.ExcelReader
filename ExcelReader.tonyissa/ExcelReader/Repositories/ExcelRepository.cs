using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Repositories;

public class ExcelRepository<TEntity, TContext> : IExcelRepository<TEntity>
    where TContext : DbContext
    where TEntity : class
{
    private readonly TContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public ExcelRepository(TContext context)
    {
        _dbSet = context.Set<TEntity>();
        _context = context;
    }

    public async Task AddNewEntry(TEntity entry)
    {
        _dbSet.Add(entry);
        await _context.SaveChangesAsync();
    }

    public async Task<TEntity[]> GetAllEntries()
    {
        return await _dbSet.ToArrayAsync();
    }
}