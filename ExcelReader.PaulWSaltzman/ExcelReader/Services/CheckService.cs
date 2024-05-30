using ExcelReader.Models;
using ExcelReader.Data;


namespace ExcelReader.Services
{
     public class CheckService
    {
        private readonly ExcelContext _context;

        public CheckService(ExcelContext context)
        {
            _context = context;
        }

        internal async Task<List<Check>> GetAllChecks()
        {
            return _context.Checks.ToList();
        }

        internal async Task InsertChecks(List<Check> checks)
        {
            _context.Checks.AddRange(checks);
            _context.SaveChanges();
        }

        internal async Task<bool> CheckDatabase()
        {
            bool databaseExists = _context.Database.CanConnect();
            return databaseExists;
        }

        internal async Task DropDatabaseAsync()
        {
            await _context.Database.EnsureDeletedAsync();
        }

        internal async Task CreateDatabaseAsync()
        {
            await _context.Database.EnsureCreatedAsync();
        }
    }
}
