using ExcelReader.Arashi256.Models;
using ExcelReader.Arashi256.Classes;

namespace ExcelReader.Arashi256.Interfaces
{
    public interface IDatabaseService
    {
        Task<ServiceResponse> AddMovie(Movie movie);
        Task<ServiceResponse> GetMovies();
    }
}