using System.Configuration;
using System.Data;

var connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

// Your database name
string databaseName = "ArquiveReader";
var file = ConfigurationManager.AppSettings.Get("FilePath");

Console.WriteLine("Creating Database...");

// Drop the database if it exists
DropDatabaseIfExists(connectionString, databaseName);

DataTable columnHeaders = GetHeaders(file);

// Create the database
CreateDatabase(connectionString, databaseName);
CreateTable(connectionString, databaseName, columnHeaders);

Console.WriteLine("Database created");
Console.WriteLine("Saving to the database....");

// Load Excel data into a DataTable
// Save data to the database
PopulateTable(file, connectionString, columnHeaders, databaseName);

Console.WriteLine("Save sucessfull. Loading table!\n");

ShowTable();