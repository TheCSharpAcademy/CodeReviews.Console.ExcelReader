using ExcelReader.Arashi256.Classes;
using ExcelReader.Arashi256.Interfaces;
using ExcelReader.Arashi256.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.Arashi256.Services
{
    internal class DatabaseService : IDatabaseService
    {
        private readonly ExcelReaderContext _context;
        public DatabaseService(ExcelReaderContext dbContext) 
        { 
            _context = dbContext;
        }
        public async Task<ServiceResponse> AddMovie(Movie movie) 
        {
            try
            {
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", movie);
            }
            catch (DbUpdateException ex)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
            }
        }
        public async Task<ServiceResponse> GetMovies()
        {
            List<Movie> movies = await _context.Movies.ToListAsync();
            if (movies != null && movies.Count > 0)
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", movies);
            else
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "No movies found", movies);
        }
    }
}
