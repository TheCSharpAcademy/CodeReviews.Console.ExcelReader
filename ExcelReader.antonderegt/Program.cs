using ExcelReader;
using Microsoft.Extensions.Configuration;

List<Number> numbers = ExcelFileReader.ReadExcel("Numbers.xlsx");

Configuration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
Database database = new(configuration);
database.Initialize();
await database.AddToDatabase(numbers);
await database.ReadFromDatabase();