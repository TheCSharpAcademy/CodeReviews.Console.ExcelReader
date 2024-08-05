using ExcelReader.kwm0304.Config;
using ExcelReader.kwm0304.Data;

namespace ExcelReader.kwm0304;

public class Program
{
  public static void Main()
  {
    var connString = AppConfiguration.GetConnectionString("DefaultConnection");
    var dbAccess = new DatabaseAccess(connString);
    var session = new AppSession(dbAccess);
    session.OnStart();
  }
}