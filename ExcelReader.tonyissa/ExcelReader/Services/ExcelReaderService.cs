namespace ExcelReader.Services;

public class ExcelReaderService
{
    private readonly ILogger<ExcelReaderService> _logger;
    private readonly FileProcesserService _fileProcesserService;
    private readonly string[] _filenames;

    public ExcelReaderService(ILogger<ExcelReaderService> logger, FileProcesserService fileProcesserService)
    {
        _logger = logger;
        _fileProcesserService = fileProcesserService;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
        _filenames = Directory.GetFiles(path, "*.xls?");

        if (_filenames.Length == 0)
            throw new FileNotFoundException(".xls or .xlsx files not found in Files folder. Please add some and then try again.");
    }

    public async Task ExecuteServiceAsync()
    {
        _logger.LogInformation("FileProcesser running at: {time}", DateTimeOffset.Now.ToString("g"));

        var results = new List<Task>();

        for (int i = 0; i < _filenames.Length; i++)
        {
            results.Add(Task.Run(async () =>
            {
                _logger.LogInformation("Service {iter}/{max} starting at: {time}",
                    i + 1, _filenames.Length, DateTimeOffset.Now.ToString("g"));

                await _fileProcesserService.ProcessFilesAsync(_filenames[i]);

                _logger.LogInformation("Service {iter}/{max} stopping at: {time}",
                    i + 1, _filenames.Length, DateTimeOffset.Now.ToString("g"));
            }));
        }

        await Task.WhenAll(results);

        _logger.LogInformation("FileProcesser stopping at: {time}", DateTimeOffset.Now.ToString("g"));
    }
}
