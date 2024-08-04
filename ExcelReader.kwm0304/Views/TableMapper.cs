using System.Reflection;

namespace ExcelReader.kwm0304.Views;

public class TableMapper<T>
{
  public string[] CerateColumnNames()
  {
    return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
    .Select(e => e.Name)
    .ToArray();
  }

  public string[] CreateRowValues(T entity)
  {
    return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
      .Select(e => TableMapper<T>.FormatValue(e.GetValue(entity)!))
      .ToArray();
  }

  private static string FormatValue(object v)
  {
    if (v is DateTime dateTime)
    {
      return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
    return v?.ToString() ?? "-";
  }
}