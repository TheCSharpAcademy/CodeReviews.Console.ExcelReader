using ExcelReader;
using ExcelReader.Controllers;
using ExcelReader.Database;
using ExcelReader.Services;
using ExcelReader.Views;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

Run();

static void Run()
{
    AnsiConsole.MarkupLine($"\n\t\t[bold][green]E X C E L[/]    [blue]R E A D E R[/][/]\n");

    var serviceProvider = Bootstrap();
    var controller = serviceProvider.GetService<Controller>();
    var dataPersistenceService = serviceProvider.GetService<DataPersistenceService>();

    if (serviceProvider == null || controller == null || dataPersistenceService == null)
    {
        throw new Exception("Could not initialise application");
    }

    var (path, hasHeader) = View.PromptForFile();

    AnsiConsole.MarkupLine($"[blue]Extracting data from file...[/]");
    AnsiConsole.MarkupLine($"\tPath: {path}");
    AnsiConsole.MarkupLine($"\tPreparing rows, columns and cell values ...");

    var parsedResult = ExcelParser.Parse(path, hasHeader);

    if (parsedResult == null || parsedResult.ErrorMessage != null)
    {
        AnsiConsole.MarkupLine($"[red]{parsedResult?.ErrorMessage ?? "Could not read file"}[/]");
        Environment.Exit(1);
    }

    AnsiConsole.MarkupLine($"\tExtracted [yellow]{parsedResult.ParsedCols.Count} columns[/]" +
        $", [yellow]{parsedResult.ParsedRows.Count} rows[/]");
    AnsiConsole.MarkupLine("\t[green]Done[/]");

    AnsiConsole.Markup($"[blue]Saving extracted data in database...[/]");
    dataPersistenceService.InsertParsedData(parsedResult);
    AnsiConsole.MarkupLine("[green]Done[/]");

    AnsiConsole.Markup($"[blue]Retrieving data from database...[/]");
    var (rows, cols) = controller.FetchSerializedData();
    AnsiConsole.MarkupLine("[green]Done[/]");

    AnsiConsole.MarkupLine($"[blue]Displaying data...[/]\n");
    View.DisplayData(rows, cols);
    AnsiConsole.MarkupLine("[green]Done[/]");

    AnsiConsole.MarkupLine("\n\n[blue]Exiting app...[/]");
}

static ServiceProvider Bootstrap()
{
    var serviceProvider = new ServiceCollection()
        .AddSingleton<ExcelReaderDbContext>()
        .AddSingleton<DataPersistenceService>()
        .AddSingleton<Controller>()
        .AddSingleton<SetupService>()
        .BuildServiceProvider();

    var setupService = serviceProvider.GetService<SetupService>();
    setupService?.Setup();

    return serviceProvider;
}