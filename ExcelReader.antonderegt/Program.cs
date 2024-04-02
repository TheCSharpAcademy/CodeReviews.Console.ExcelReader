using ExcelReader;

List<Number> numbers = ExcelFileRead.ReadXLS("Numbers.xlsx");

Database database = new();
database.Initialize();
await database.AddToDatabase(numbers);
await database.ReadFromDatabase();