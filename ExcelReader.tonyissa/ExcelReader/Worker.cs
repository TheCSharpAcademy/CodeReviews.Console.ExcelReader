namespace ExcelReader;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly string[] _filenames;

    public Worker(ILogger<Worker> logger, IConfiguration config)
    {
        _logger = logger;
        _filenames = config.GetSection("ExcelFilenames").Get<string[]>();

        if (_filenames == null) 
            throw new ArgumentNullException(nameof(config), "No filenames found. Please update configuration.");

        var path = Path.Combine(Directory.GetCurrentDirectory(), "Data");

        foreach (var filename in _filenames)
        {
            var fileLocation = Path.Combine(path, filename);

            if (!Path.Exists(fileLocation)) 
                throw new FileNotFoundException($"Could not find {filename}");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now.ToString("F"));

        await ProcessFilesAsync(stoppingToken);

        _logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now.ToString("F"));
    }

    private async Task ProcessFilesAsync(CancellationToken stoppingToken)
    {
        for (int i = 0; i < _filenames.Length; i++)
        {
            if (stoppingToken.IsCancellationRequested)
                return;

            _logger.LogInformation("Service {iter}/{max} starting at: {time}", 
                i + 1, _filenames.Length, DateTimeOffset.Now.ToString("F"));

            // await doSomething(file);

            _logger.LogInformation("Service {iter}/{max} stopping at: {time}", 
                i + 1, _filenames.Length, DateTimeOffset.Now.ToString("F"));
        }
    }
}
