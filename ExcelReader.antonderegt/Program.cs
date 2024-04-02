using ExcelReader;

List<Number> numbers = ExcelFileReader.ReadExcel("Numbers.xlsx");

Database database = new();
database.Initialize();
await database.AddToDatabase(numbers);
await database.ReadFromDatabase();