using ExcelReader;
using ExcelReader.Model;
using Spectre.Console;

ExcelReaderContext context = new();
DatabaseInteraction database = new(context);
Reader reader = new("F:\\Visual repos\\Study\\ExcelReader\\ExcelReader\\TableFiles\\SheetExcelReaderSample.xlsx");

// The Thread.Sleep are there only to add time to see the console display
// They have no functionnal purpose and can be discarded
AnsiConsole.Status()
    .Start("Accessing the file", ctx =>
    {
        AnsiConsole.MarkupLine("Reading the sheet...");
        List<Aliment> aliments = reader.ParseFile();
        Thread.Sleep(1000);

        ctx.Status("Accessing Database");
        ctx.Spinner(Spinner.Known.Star);
        ctx.SpinnerStyle(Style.Parse("green"));
        Thread.Sleep(1000);

        AnsiConsole.MarkupLine("Seeding Database...");
        database.SeedDatabaseFromList(aliments);
        Thread.Sleep(1000);

        ctx.Status("Preparing Display");
        ctx.Spinner(Spinner.Known.Star);
        ctx.SpinnerStyle(Style.Parse("green"));
        Thread.Sleep(1000);

        AnsiConsole.MarkupLine("Preparing the food...");
        List<Aliment> theFood = database.GetRandomAliments();
        Thread.Sleep(1000);

        foreach (Aliment aliment in theFood)
        {
            Console.WriteLine("");
            Console.WriteLine($"Name: {aliment.Name}");
            Console.WriteLine($"Calories for 100g: {aliment.Calories}");
            Console.WriteLine($"Protein for 100g: {aliment.Proteines}");
        }
        Console.WriteLine();
        Console.WriteLine();
    });

Console.ReadLine();