
using exelReader2._0;
using exelReader2._0.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Spectre.Console;

Test();

static void Test()
{
    List<Person> persons = new();

    var filePath = Path.Combine(Directory.GetCurrentDirectory(),"Libro.xlsx");

    string path = Directory.GetCurrentDirectory();
    Console.WriteLine(path);

    Console.WriteLine("\tExcel Reader.");
    Console.WriteLine($"File Path: {filePath}");
    Console.WriteLine();

    FileInfo existingFile = new FileInfo(filePath);

    using (ExcelPackage package = new ExcelPackage(existingFile))
    {
        //Get the first worksheet in the workbook
        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        int numOfRows = worksheet.Dimension.Rows;//getting numbers of row inside worksheed

        //getting a List of Person model; List<Person>
        for (int row = 2; row <= numOfRows; row++)
        {
            persons.Add(
                new Person
                {
                    Name = worksheet.Cells[row, 1].Value.ToString(),
                    SecondName = worksheet.Cells[row, 2].Value.ToString(),
                    City = worksheet.Cells[row, 3].Value.ToString(),
                    Mail = worksheet.Cells[row, 4].Value.ToString(),
                    Phone = worksheet.Cells[row, 5].Value.ToString(),
                });
        }

    }
    Console.WriteLine("Saving to DB");
    Persist(persons);//saving to DB
    Console.WriteLine("Done!");

    Console.WriteLine("\nPrinting Persons in Database.");
    var personsFromDb = Get();
    PrintPersons(personsFromDb);
}

static void Persist(List<Person> persons)
{


    //create db if does not exist
    ContextDB db = new();
    if (db.Database.CanConnect())
    {
        Console.WriteLine("Data Base does exist!\nDeleting database and creating a new one!");
        db.Database.EnsureDeleted();
        db.Database.Migrate();
        Console.WriteLine("Database created successfully.");
    }
    else
    {
        Console.WriteLine("Database does not exist!");
        db.Database.Migrate();
        Console.WriteLine("Database created successfully.");

    }


    db.Persons.AddRange(persons);
    db.SaveChanges();
}
 
static List<Person> Get()
{
    using (ContextDB db = new())
    {
        return db.Persons.ToList();
    }
}

static void PrintPersons(IEnumerable<Person> persons)
{
    var table = new Table();
    table.AddColumn("Id");
    table.AddColumn("Name");
    table.AddColumn("Second Name");
    table.AddColumn("City");
    table.AddColumn("Mail");
    table.AddColumn("Phone Number");

    foreach (var p in persons)
    {
        table.AddRow(p.Id.ToString(),
            p.Name,
            p.SecondName,
            p.City,
            p.Mail,
            p.Phone);
    }

    AnsiConsole.Write(table);
    Console.Write("Press any Key to continue");
    Console.ReadLine();
}