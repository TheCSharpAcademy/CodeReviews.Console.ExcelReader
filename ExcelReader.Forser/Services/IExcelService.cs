namespace ExcelReader.Forser.Services
{
    internal interface IExcelService
    {
        void AddPlayers(IEnumerable<HockeyModel> hockeyPlayers);
        List<HockeyModel> DisplayAllPlayers();
    }
}