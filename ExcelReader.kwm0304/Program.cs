// using ExcelReader.kwm0304.Config;
using ExcelReader.kwm0304.Data;

namespace ExcelReader.kwm0304;
// This is an application that will read data from an Excel spreadsheet into a database
//  When the application starts, it should delete the database if it exists, create a new one, create all tables, read from Excel, seed into the database.
//  You need to use EPPlus package
//  You shouldn't read into Json first.
//  You can use SQLite or SQL Server (or MySQL if you're using a Mac)
//  Once the database is populated, you'll fetch data from it and show it in the console.
//  You don't need any user input
//  You should print messages to the console letting the user know what the app is doing at that moment (i.e. reading from excel; creating tables, etc)
//  The application will be written for a known table, you don't need to make it dynamic.
//  When submitting the project for review, you need to include an xls file that can be read by your application.
public class Program
{
  public static void Main(string[] args)
  {
    // var connString = AppConfiguration.GetConnectionString("Server=localhost;Database=csvReaderDb;Trusted_Connection=True;TrustServerCertificate=True;");
    var dbAccess = new DatabaseAccess("Server=localhost;Database=csvReaderDb;Trusted_Connection=True;TrustServerCertificate=True;");
    var session = new AppSession(dbAccess);
    session.OnStart();
  }
}
