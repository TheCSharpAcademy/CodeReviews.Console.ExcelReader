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
        _logger.LogInformation("ExcelReaderService running at: {time}", DateTimeOffset.Now.ToString("g"));

        for (int i = 1; i <= _filenames.Length; i++)
        {
            var currentIteration = i;

            _logger.LogInformation("FileProcesserService {iter}/{max} starting at: {time}",
                currentIteration, _filenames.Length, DateTimeOffset.Now.ToString("g"));

            await _fileProcesserService.ProcessFilesAsync(_filenames[currentIteration - 1]);

            _logger.LogInformation("FileProcesserService {iter}/{max} stopping at: {time}",
                currentIteration, _filenames.Length, DateTimeOffset.Now.ToString("g"));
        }

        await _fileProcesserService.DisplayListAfterFinished();

        _logger.LogInformation("ExcelReaderService stopping at: {time}", DateTimeOffset.Now.ToString("g"));
    }
}
