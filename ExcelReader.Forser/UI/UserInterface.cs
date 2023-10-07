using Spectre.Console;

namespace ExcelReader.Forser.UI
{
    public class UserInterface : IUserInterface
    {
        public void DisplayAllPlayers(List<HockeyModel> hockeyPlayers)
        {
            Table playerTable = new Table();
            playerTable.Expand();
            playerTable.AddColumns("Id", "Team", "Country", "Firstname", "Lastname", "Weight", "Date of Birth", "Hometown", "Provinces", "Position", "Age", "HeightFt", "Htln", "BMI");

            foreach (HockeyModel hockeyPlayer in hockeyPlayers)
            {
                playerTable.AddRow($"{hockeyPlayer.Id}", $"{hockeyPlayer.Team}", $"{hockeyPlayer.Country}", $"{hockeyPlayer.FirstName}", $"{hockeyPlayer.LastName}", $"{hockeyPlayer.Weight}", 
                    $"{DateTime.Parse(hockeyPlayer.DateOfBirth).ToShortDateString()}", $"{hockeyPlayer.HomeTown}", $"{hockeyPlayer.Provinces}", $"{hockeyPlayer.Position}", $"{hockeyPlayer.Age}", 
                    $"{string.Format("{0:0.00}", double.Parse(hockeyPlayer.HeightFt))}", $"{hockeyPlayer.Htln}", $"{hockeyPlayer.BMI}");
            }

            AnsiConsole.Write(playerTable);
        }
        public void RenderTitle(string message)
        {
            Rule renderTitle = new Rule($"[blue]{message}[/]");
            AnsiConsole.Write(renderTitle);
        }
    }
}