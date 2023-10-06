namespace ExcelReader.Forser.Repository
{
    internal interface IExcelRepository
    {
        public IEnumerable<HockeyModel> GetAllPlayers();
        public void AddPlayer(HockeyModel hockeyPlayer);
    }
}