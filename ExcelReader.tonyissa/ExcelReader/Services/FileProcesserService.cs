namespace ExcelReader.Services;

public class FileProcesserService
{
    private readonly ILogger<FileProcesserService> _logger;

    public FileProcesserService(ILogger<FileProcesserService> logger)
    {
        _logger = logger;
    }


}