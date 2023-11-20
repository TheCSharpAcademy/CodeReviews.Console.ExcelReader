using ExcelReader.Data;
using ExcelReader.Models;
using Microsoft.Extensions.DependencyInjection;



// Setup DI container
var serviceProvider = new ServiceCollection()
    .AddDbContext<CarContext>()
    .BuildServiceProvider();


// Resolve and use your DbContext
using (var context = serviceProvider.GetService<CarContext>())
{
    try
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Console.WriteLine("---------------------");
        Console.WriteLine("Reading Excel data");
        Console.WriteLine("---------------------");

        var cars = ReadExcelData.Read();

        Console.WriteLine("---------------------");
        Console.WriteLine("Done reading data from Excel table");
        Console.WriteLine("---------------------");

        Console.WriteLine("Inserting data to db");
        context.Cars.AddRange(cars);

        Console.WriteLine("Saving data to db");
        context.SaveChanges();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Fail to delete, create or inserting db");
    }

    var carsFromDb = context.Cars.ToList();

    Console.WriteLine("---------------------");
    Console.WriteLine("Reading data from db");
    Console.WriteLine("---------------------");

    foreach (var car in carsFromDb)
    {
        Console.WriteLine(car);
    }
}