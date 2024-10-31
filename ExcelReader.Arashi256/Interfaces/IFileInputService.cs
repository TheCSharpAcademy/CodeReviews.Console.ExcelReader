using ExcelReader.Arashi256.Classes;

namespace ExcelReader.Arashi256.Interfaces
{
    public interface IFileInputService
    {
        Task<ServiceResponse> LoadMovies(string filePath);
    }
}