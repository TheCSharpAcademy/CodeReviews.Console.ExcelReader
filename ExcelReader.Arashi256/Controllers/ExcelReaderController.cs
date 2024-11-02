using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using ExcelReader.Arashi256.Models;
using ExcelReader.Arashi256.Config;
using ExcelReader.Arashi256.Classes;
using ExcelReader.Arashi256.Interfaces;

namespace ExcelReader.Arashi256.Controllers
{
    internal class ExcelReaderController : IHostedService
    {
        private readonly ILogger<ExcelReaderController> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ExcelReaderContext _dbContext;
        private readonly AppManager _appManager;
        private readonly IFileInputService _fileInputService;
        private readonly IDatabaseService _databaseService;
        private readonly IFileOutputService _fileOutputService;

        public ExcelReaderController(AppManager appManager, ILogger<ExcelReaderController> logger, IHostApplicationLifetime appLifetime, ExcelReaderContext dbContext, IFileInputService fs, IDatabaseService ds, IFileOutputService os)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _dbContext = dbContext;
            _appManager = appManager;
            _fileInputService = fs;
            _databaseService = ds;
            _fileOutputService = os;
        }

        private async Task Run()
        {
            var errors = new List<string>();
            _logger.LogInformation("Starting controller task...");
            // Get the external data file path from appsettings.json via the AppManager.
            var inputFilePath = _appManager.GetDataFileInputPath();
            if (inputFilePath == null)
            {
                // Log an error or handle the missing file path scenario
                _logger.LogError("Data file location is null.");
                return;
            }
            var response = await _fileInputService.LoadMovies(inputFilePath);
            if (response == null)
            {
                _logger.LogError("Null response from file service");
                return;
            }
            // Check we have some data
            if (response.Status.Equals(ResponseStatus.Success))
            {
                _logger.LogInformation("External file response success!");
                var data = response.Data as List<Movie>;
                if (data != null)
                {
                    _logger.LogInformation("Got sensible data back from the external file...");
                    // Add to the database.
                    foreach (var movie in data)
                    {
                        response = await _databaseService.AddMovie(movie);
                        if (response.Status.Equals(ResponseStatus.Failure)) _logger.LogError(response.Message);
                    }
                    // Prepare success response with possible errors on import.
                    var message = errors.Count == 0 ? "Added to database: OK" : $"Added to database with errors. Issues: {string.Join("; ", errors)}";
                    _logger.LogInformation(message);
                    // Retrieve what we've just added from the database.
                    _logger.LogInformation("Attempting to retrieve the data from the database...");
                    response = await _databaseService.GetMovies();
                    // Display the database data via the logger.
                    if (response.Status.Equals(ResponseStatus.Success))
                    {
                        var movies = response.Data as List<Movie>;
                        if (movies != null)
                        {
                            _logger.LogInformation("Got data response! Preparing to list database data...");
                            // List out the database data
                            ListMovies(movies);
                            var outputFilePath = _appManager.GetDataFileOutputPath();
                            if (string.IsNullOrEmpty(outputFilePath))
                            {
                                _logger.LogError("Data file output path is not configured.");
                                return;
                            }
                            response = await _fileOutputService.ExportMoviesAsync(outputFilePath, movies);
                            if (response.Status.Equals(ResponseStatus.Success))
                            {
                                _logger.LogInformation($"Database data saved back to external file '{outputFilePath}': OK.");
                            }
                            else
                            {
                                _logger.LogError($"ERROR: Could not save database data in output file: '{response.Message}'");
                            }
                        }
                        else
                            _logger.LogError($"ERROR: Although response was successful, could not get database data: '{response.Message}'");
                    }
                }
                else
                {
                    _logger.LogError($"ERROR: Got response from file service, but the data is empty: {response.Message}");
                }
            }
            else
            {
                _logger.LogError($"ERROR: {response.Message}");
            }
            _logger.LogInformation("Controller task completed.");
        }

        private void ListMovies(List<Movie> movies)
        {
            foreach (var movie in movies) 
            {
                _logger.LogInformation($"{movie}");
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(ExcelReaderController)}...");
                // Drop and recreate the database
                _dbContext.Database.EnsureDeleted();
                _dbContext.Database.EnsureCreated();
                await Run();
            }
            finally
            {
                // Gracefully shut down the application once Run() completes
                _appLifetime.StopApplication();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {nameof(ExcelReaderController)} with {cancellationToken}");
            return Task.CompletedTask;
        }
    }
}