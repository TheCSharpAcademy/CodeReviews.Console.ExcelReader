using ExcelReader.Configurations;
using ExcelReader.ConsoleApp.Engines;
using ExcelReader.Constants;
using ExcelReader.Models;
using ExcelReader.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;

namespace ExcelReader.ConsoleApp;

/// <summary>
/// A ConsoleApplication implemented as a HostedService.
/// </summary>
internal class App : IHostedService
{
    #region Fields

    private readonly IHostApplicationLifetime _appLifetime;
    private readonly ILogger<App> _logger;
    private readonly ApplicationOptions _options;
    private readonly IDataManager _databaseService;
    private readonly IDataFileProcessor _dataFileProcessor;
    private int? _exitCode;

    #endregion
    #region Constructors

    public App(
        IHostApplicationLifetime appLifetime,
        ILogger<App> logger,
        IOptions<ApplicationOptions> options,
        IDataManager databaseService,
        IDataFileProcessor dataFileProcessor)
    {
        _appLifetime = appLifetime;
        _logger = logger;
        _options = options.Value;
        _databaseService = databaseService;
        _dataFileProcessor = dataFileProcessor;
    }

    #endregion
    #region Properties

    public string DoneDirectoryPath => Path.GetFullPath(Path.Combine(_options.WorkingDirectoryPath, DirectoryName.Done));

    public string ErrorDirectoryPath => Path.GetFullPath(Path.Combine(_options.WorkingDirectoryPath, DirectoryName.Error));

    public string IncomingDirectoryPath => Path.GetFullPath(Path.Combine(_options.WorkingDirectoryPath, DirectoryName.Incoming));

    public string ProcessingDirectoryPath => Path.GetFullPath(Path.Combine(_options.WorkingDirectoryPath, DirectoryName.Processing));

    #endregion
    #region Methods - Public

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    // Configure working directory.
                    ConfigureWorkingDirectory();

                    // Configure database.
                    _databaseService.ResetDatabase();

                    // Process files in the incoming directory.
                    List<DataFile> incomingDataFiles = ProcessIncomingDirectory();

                    // Process database - ADD.
                    await AddIncomingDataFilesToDatabase(incomingDataFiles);

                    // Process database - GET.
                    var processedDataFiles = await GetProcessedDataFilesFromDatabase();

                    // Process reports.
                    DisplayDataFiles(processedDataFiles);

                    _logger.LogInformation("Press any key to continue...");
                    Console.ReadKey();
                    _exitCode = 0;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Exception Message = {ExceptionMessage}", exception.Message);
                    _exitCode = 1;
                }
                finally
                {
                    _appLifetime.StopApplication();
                }
            });
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
        return Task.CompletedTask;
    }

    #endregion
    #region Methods - Private

    private async Task AddIncomingDataFilesToDatabase(List<DataFile> incomingDataFiles)
    {
        _logger.LogInformation("Starting {method}", nameof(AddIncomingDataFilesToDatabase));

        foreach (var dataFile in incomingDataFiles)
        {
            dataFile.Id = await _databaseService.CreateAsync(dataFile);
            foreach (var dataSheet in dataFile.DataSheets)
            {
                dataSheet.DataFileId = dataFile.Id;
                dataSheet.Id = await _databaseService.CreateAsync(dataSheet);
                foreach (var dataField in dataSheet.DataFields)
                {
                    dataField.DataSheetId = dataSheet.Id;
                    dataField.Id = await _databaseService.CreateAsync(dataField);
                }
                foreach (var dataSheetRow in dataSheet.DataSheetRows)
                {
                    dataSheetRow.DataSheetId = dataSheet.Id;
                    dataSheetRow.Id = await _databaseService.CreateAsync(dataSheetRow);
                    foreach (var dataItem in dataSheetRow.DataItems)
                    {
                        dataItem.DataFieldId = dataSheet.DataFields.First(x => x.Position == dataItem.Position).Id;
                        dataItem.DataSheetRowId = dataSheetRow.Id;
                        dataItem.Id = await _databaseService.CreateAsync(dataItem);
                    }
                }
            }
        }

        _logger.LogInformation("Finished {method}", nameof(AddIncomingDataFilesToDatabase));
    }

    private void ConfigureWorkingDirectory()
    {
        _logger.LogInformation("Starting {method}", nameof(ConfigureWorkingDirectory));

        foreach (var property in GetType().GetProperties())
        {
            if (property.Name.EndsWith("DirectoryPath"))
            {
                var directoryPath = property.GetValue(this)?.ToString();
                if (!string.IsNullOrWhiteSpace(directoryPath) && !Directory.Exists(directoryPath))
                {
                    _logger.LogInformation("Creating {directory}", directoryPath);
                    Directory.CreateDirectory(directoryPath);
                }
            }
        }

        _logger.LogInformation("Finished {method}", nameof(ConfigureWorkingDirectory));
    }

    private void DisplayDataFiles(IReadOnlyList<DataFile> processedDataFiles)
    {
        _logger.LogInformation("Starting {method}", nameof(DisplayDataFiles));

        foreach (var dataFile in processedDataFiles)
        {
            foreach (var dataSheet in dataFile.DataSheets)
            {
                var title = $"File: {dataFile.Name} Sheet: {dataSheet.Name}";
                var table = TableEngine.GetTable(title, dataSheet);
                AnsiConsole.Write(table);
            }
        }
        _logger.LogInformation("Finished {method}", nameof(DisplayDataFiles));
    }

    private async Task<IReadOnlyList<DataFile>> GetProcessedDataFilesFromDatabase()
    {
        _logger.LogInformation("Starting {method}", nameof(GetProcessedDataFilesFromDatabase));

        var dataFiles = await _databaseService.GetDataFilesAsync();
        foreach (var dataFile in dataFiles)
        {
            dataFile.DataSheets.AddRange(await _databaseService.GetDataSheetsByWorkbookIdAsync(dataFile.Id));

            foreach (var dataSheet in dataFile.DataSheets)
            {
                dataSheet.DataFields.AddRange(await _databaseService.GetDataFieldsByWorksheetIdAsync(dataSheet.Id));
                dataSheet.DataSheetRows.AddRange(await _databaseService.GetDataSheetRowsByWorksheetIdAsync(dataSheet.Id));

                foreach (var dataSheetRow in dataSheet.DataSheetRows)
                {
                    dataSheetRow.DataItems.AddRange(await _databaseService.GetDataItemsByRowIdAsync(dataSheetRow.Id));
                }
            }
        }
        _logger.LogInformation("Finished {method}", nameof(GetProcessedDataFilesFromDatabase));
        return dataFiles;
    }

    private List<DataFile> ProcessIncomingDirectory()
    {
        _logger.LogInformation("Starting {method}", nameof(ProcessIncomingDirectory));

        List<DataFile> incomingDataFiles = [];

        foreach (var fileInfo in new DirectoryInfo(IncomingDirectoryPath).EnumerateFiles())
        {
            _logger.LogInformation("Processing file {file}", fileInfo.Name);

            fileInfo.MoveTo(Path.Combine(ProcessingDirectoryPath, fileInfo.Name), true);

            try
            {
                incomingDataFiles.Add(_dataFileProcessor.ProcessFile(fileInfo));

                fileInfo.MoveTo(Path.Combine(DoneDirectoryPath, fileInfo.Name), true);

                _logger.LogInformation("File processed");
            }
            catch (Exception exception)
            {
                _logger.LogWarning("Error procesing file {message}", exception.Message);

                fileInfo.MoveTo(Path.Combine(ErrorDirectoryPath, fileInfo.Name), true);

                _logger.LogInformation("File aborted");
            }
        }

        _logger.LogInformation("Finished {method}", nameof(ProcessIncomingDirectory));
        return incomingDataFiles;
    }

    #endregion
}
