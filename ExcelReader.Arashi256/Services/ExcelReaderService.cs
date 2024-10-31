using ExcelReader.Arashi256.Models;
using ExcelReader.Arashi256.Classes;
using ExcelReader.Arashi256.Interfaces;
using OfficeOpenXml;

public class ExcelReaderService : IFileInputService
{
    public async Task<ServiceResponse> LoadMovies(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "File path to data file cannot be empty!", null);
        }
        var movies = new List<Movie>();
        var errors = new List<string>();
        try
        {
            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, $"File not found at file path: '{filePath}'", null);
            }
            using var package = new ExcelPackage(fileInfo);
            await package.LoadAsync(fileInfo);
            var worksheet = package.Workbook.Worksheets[0];
            int rowCount = worksheet.Dimension.Rows;
            for (int row = 2; row <= rowCount; row++)
            {
                try
                {
                    var movie = new Movie
                    {
                        Id = int.Parse(worksheet.Cells[row, 1].Text),
                        Name = worksheet.Cells[row, 2].Text,
                        Year = worksheet.Cells[row, 3].Text,
                        Runtime = worksheet.Cells[row, 4].Text,
                        ImdbRating = decimal.Parse(worksheet.Cells[row, 5].Text)
                    };
                    movies.Add(movie);
                }
                catch (FormatException fe)
                {
                    errors.Add($"Data format issue at row {row}: {fe.Message}");
                }
                catch (OverflowException oe)
                {
                    errors.Add($"Numeric overflow at row {row}: {oe.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
        }
        // Prepare success response with possible errors on import.
        var message = errors.Count == 0 ? "OK" : $"Imported with errors. Issues: {string.Join("; ", errors)}";
        return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, message, movies);
    }
}