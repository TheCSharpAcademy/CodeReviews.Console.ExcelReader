namespace ExcelReader.Services;

public class FileProcesser
{
    private readonly ILogger<FileProcesser> _logger;
    private readonly string[] _filenames;

    public FileProcesser(ILogger<FileProcesser> logger)
    {
        _logger = logger;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
        _filenames = Directory.GetFiles(path, "*.xls?");

        if (_filenames.Length == 0)
            throw new FileNotFoundException(".xls or .xlsx files not found in Files folder. Please add some and then try again.");
    }

    public async Task ProcessFilesAsync()
    {
        _logger.LogInformation("FileProcesser running at: {time}", DateTimeOffset.Now.ToString("g"));

        for (int i = 0; i < _filenames.Length; i++)
        {
            _logger.LogInformation("Service {iter}/{max} starting at: {time}",
                i + 1, _filenames.Length, DateTimeOffset.Now.ToString("g"));

            // await doSomething(file);

            _logger.LogInformation("Service {iter}/{max} stopping at: {time}",
                i + 1, _filenames.Length, DateTimeOffset.Now.ToString("g"));
        }

        _logger.LogInformation("FileProcesser stopping at: {time}", DateTimeOffset.Now.ToString("g"));
    }
}
