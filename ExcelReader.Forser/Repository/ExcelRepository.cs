using ExcelReader.Forser.Context;

namespace ExcelReader.Forser.Repository
{
    internal class ExcelRepository : IExcelRepository
    {
        private readonly ExcelContext _excelContext;

        public ExcelRepository(ExcelContext excelContext)
        {
            _excelContext = excelContext;
        }
        public void AddPlayer(HockeyModel hockeyPlayer)
        {
            try
            {
                _excelContext.Add(hockeyPlayer);
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