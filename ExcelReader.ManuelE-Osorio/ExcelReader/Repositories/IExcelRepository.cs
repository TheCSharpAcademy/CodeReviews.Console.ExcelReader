namespace ExcelReader.Repositories;

public interface IExcelRepository<T> where T : class
{
    bool TryConnection();
    bool Insert(T model);
    IEnumerable<T>? GetAll();
    T? GetById(int id);
    bool Update(T model);
    bool Delete(T model);
}