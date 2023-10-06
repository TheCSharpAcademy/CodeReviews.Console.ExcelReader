namespace ExcelReader.Forser.UI
{
    internal interface IUserInterface
    {
        void RenderTitle(string message);
        void DisplayAllPlayers(List<HockeyModel> hockeyPlayers);
    }
}