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
    var serviceProvider = Bootstrap();
    var controller = serviceProvider.GetService<Controller>();
    var dataPersistenceService = serviceProvider.GetService<DataPersistenceService>();

    if (serviceProvider == null || controller == null || dataPersistenceService == null)
    {
        throw new Exception("Could not initialise application");
    }

    var (path, hasHeader) = View.PromptForFile();

    var parsedResult = ExcelParser.Parse(path, hasHeader);

    if (parsedResult == null || parsedResult.ErrorMessage != null)
    {
        AnsiConsole.MarkupLine($"[red]{parsedResult?.ErrorMessage ?? "Could not read file"}[/]");
        Environment.Exit(1);
    }

    dataPersistenceService.InsertParsedData(parsedResult);

    var (rows, cols) = controller.FetchSerializedData();

    View.DisplayData(rows, cols);
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