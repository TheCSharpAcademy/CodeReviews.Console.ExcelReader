using Microsoft.Extensions.Configuration;

namespace ExcelReader.Arashi256.Config
{
    internal class AppManager
    {
        private readonly IConfiguration _configuration;

        public AppManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? GetDataFileInputPath()
        {
            return _configuration["Data_File:Input"];
        }

        public string? GetDataFileOutputPath()
        {
            return _configuration["Data_File:Output"];
        }

        public string? GetConnectionString()
        {
            return _configuration["Database:ConnectionString"];
        }
    }
}