namespace ExcelReader.Forser.Services
{
    internal interface IExcelService
    {
        void AddPlayer(HockeyModel hockeyPlayer);
        List<HockeyModel> DisplayAllPlayers();
    }
}