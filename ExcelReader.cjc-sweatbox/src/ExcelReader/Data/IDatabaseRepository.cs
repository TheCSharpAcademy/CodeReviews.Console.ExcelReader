namespace ExcelReader.Data.Repositories;

/// <summary>
/// Definition of the database repository that must be implemented.
/// </summary>
public interface IDatabaseRepository
{
    string ConnectionString { get; }
    void EnsureCreated();
    void EnsureDeleted();
}