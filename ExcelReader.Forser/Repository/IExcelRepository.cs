namespace ExcelReader.Forser.Repository
{
    internal interface IExcelRepository
    {
        public IEnumerable<HockeyModel> GetAllPlayers();
        public void AddPlayers(IEnumerable<HockeyModel> hockeyPlayers);
    }
}