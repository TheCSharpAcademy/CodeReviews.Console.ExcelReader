using ExcelReader.BBualdo;

Console.WriteLine("Starting application...");

using (var context = new PeopleContext())
{
  Console.WriteLine("Deleting existing database...");
  context.Database.EnsureDeleted();

  Console.WriteLine("Creating new database...");
  context.Database.EnsureCreated();

  Console.WriteLine("Loading data from Excel...");
  context.LoadDataFromExcel();

  Console.WriteLine("Database has been successfully set up with the following entries:");

  ConsoleEngine.GetPeopleTable(context.People.ToList());

  Console.WriteLine("Operation completed. Exiting application...");
}