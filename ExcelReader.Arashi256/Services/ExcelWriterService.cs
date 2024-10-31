using ExcelReader.Arashi256.Classes;
using ExcelReader.Arashi256.Interfaces;
using ExcelReader.Arashi256.Models;
using OfficeOpenXml;

namespace ExcelReader.Arashi256.Services
{
    internal class ExcelWriterService : IFileOutputService
    {
        public async Task<ServiceResponse> ExportMoviesAsync(string filePath, List<Movie> movies)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "File path cannot be empty!", null);
            }
            try
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete(); // Delete existing file if it exists
                }
                using var package = new ExcelPackage(fileInfo);
                var worksheet = package.Workbook.Worksheets.Add("Movies");
                // Adding header row
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Year";
                worksheet.Cells[1, 4].Value = "Runtime";
                worksheet.Cells[1, 5].Value = "IMDB Rating";
                // Populating rows with movie data
                for (int i = 0; i < movies.Count; i++)
                {
                    var movie = movies[i];
                    worksheet.Cells[i + 2, 1].Value = movie.Id;
                    worksheet.Cells[i + 2, 2].Value = movie.Name;
                    worksheet.Cells[i + 2, 3].Value = movie.Year;
                    worksheet.Cells[i + 2, 4].Value = movie.Runtime;
                    worksheet.Cells[i + 2, 5].Value = movie.ImdbRating;
                }
                await package.SaveAsync(); // Save the Excel file
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "Movies exported successfully.", filePath);
            }
            catch (Exception ex)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
            }
        }
    }
}
