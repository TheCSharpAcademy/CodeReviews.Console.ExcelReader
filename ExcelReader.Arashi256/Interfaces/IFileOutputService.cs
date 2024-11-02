using ExcelReader.Arashi256.Classes;
using ExcelReader.Arashi256.Models;

namespace ExcelReader.Arashi256.Interfaces
{
    public interface IFileOutputService
    {
        Task<ServiceResponse> ExportMoviesAsync(string filePath, List<Movie> movies);
    }
}
