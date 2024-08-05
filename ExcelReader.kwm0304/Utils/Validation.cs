using System.Data;
using OfficeOpenXml;

namespace ExcelReader.kwm0304.Utils;

public class Validation
{
  public string NormalizeColumnName(string columnName)
  {
    return columnName.Replace(" ", "")
                     .Replace("%", "PCT")
                     .Replace("/", "_")
                     .Replace("#", "Num")
                     .Replace("-", "_");
  }

  public string SanitizeIdentifier(string identifier)
  {
    return identifier.Replace("]", "]]");
  }
  public SqlDbType GetSqlDbType(object value)
  {
    return value switch
    {
      int => SqlDbType.Int,
      long => SqlDbType.BigInt,
      float => SqlDbType.Float,
      double => SqlDbType.Float,
      decimal => SqlDbType.Decimal,
      bool => SqlDbType.Bit,
      DateTime => SqlDbType.DateTime,
      _ => SqlDbType.NVarChar,
    };
  }

  public string JoinColumnName(string columnName)
  {
    return columnName.Replace(" ", "");
  }

  public object GetTypedCellValue(ExcelRange cell)
  {
    return cell.Value switch
    {
      null => null!,
      double d when cell.Style.Numberformat.Format.Contains('%') => d,
      double d => d,
      int i => i,
      bool b => b,
      DateTime dt => dt,
      string s => s,
      _ => cell.Text,
    };
  }
}