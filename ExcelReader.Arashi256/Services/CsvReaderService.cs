using ExcelReader.Arashi256.Classes;
using ExcelReader.Arashi256.Interfaces;
using ExcelReader.Arashi256.Models;
using System.Globalization;

namespace ExcelReader.Arashi256.Services
{
    public class CsvReaderService : IFileInputService
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
                if (!File.Exists(filePath))
                {
                    return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, $"File not found at file path: '{filePath}'", null);
                }
                // Read CSV file line by line asynchronously
                using var reader = new StreamReader(filePath);
                int row = 0;
                string? line;
                // Skip the header row
                await reader.ReadLineAsync();
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    row++;
                    try
                    {
                        var columns = line.Split(',');
                        var movie = new Movie
                        {
                            Id = int.Parse(columns[0]),
                            Name = columns[1],
                            Year = columns[2],
                            Runtime = columns[3],
                            ImdbRating = decimal.Parse(columns[4], CultureInfo.InvariantCulture)
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
                    catch (IndexOutOfRangeException ioore)
                    {
                        errors.Add($"Missing data at row {row}: {ioore.Message}");
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
}
