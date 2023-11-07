using ExcelReader.Forser.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExcelReader.Forser.Repository
{
    internal class ExcelRepository : IExcelRepository
    {
        private readonly ExcelContext _excelContext;

        public ExcelRepository(ExcelContext excelContext)
        {
            _excelContext = excelContext;
            if((_excelContext.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            {
                Console.WriteLine("Deleting exisiting Database and then recreating it.");
                _excelContext.Database.EnsureDeleted(); //Delete the found one
                _excelContext.Database.EnsureCreated(); //Create a new one
            }
            else
            {
                Console.WriteLine("Create Database on first startup.");
                _excelContext.Database.EnsureCreated(); //Happens on first run when no Database exists at all.
            }
        }
        public void AddPlayers(IEnumerable<HockeyModel> hockeyPlayers)
        {
            try
            {
                _excelContext.Players.AddRange(hockeyPlayers);
                _excelContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
        public IEnumerable<HockeyModel> GetAllPlayers()
        {
            return _excelContext.Players;
        }
    }
}