namespace ExcelReader;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly string[] _filenames;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
        _filenames = Directory.GetFiles(path, "*.xls?");

        if (_filenames.Length == 0)
            throw new FileNotFoundException(".xls or .xlsx files not found in Files folder. Please add some and then try again.");
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
