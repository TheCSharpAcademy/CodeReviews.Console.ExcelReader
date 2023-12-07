
using ExcelReader.Models;
using OfficeOpenXml;
using System.Globalization;


ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
string filePath = "C:\\Users\\michi\\source\\repos\\ExcelReaderApp\\ExcelReader\\Data\\FileToBeRead.xlsx";


Console.WriteLine("Reading excel.");
Console.ReadLine();
var listOfPlayers = ReadDataFromExcel(filePath);

Console.WriteLine("Deleting and creating database.");
Console.ReadLine();
CreateDatabase();

Console.WriteLine("Populating database.");
Console.ReadLine();
PopulateDatabase(listOfPlayers);

Console.WriteLine("Getting and writing players from db.");
Console.ReadLine();
GetPlayers();

static void CreateDatabase()
{
    using (var context = new NBAPlayerContext())
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}

static List<NBAPlayer> ReadDataFromExcel(string filePath)
{
    var NBAPlayers = new List<NBAPlayer>();

    using (var package = new ExcelPackage(new FileInfo(filePath)))
    {
        var worksheet = package.Workbook.Worksheets[0];

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var player = new NBAPlayer()
            {
                playerName = worksheet.Cells[row, 1].Value.ToString(),
                gamesPlayed = worksheet.Cells[row, 2].GetValue<int>(),
                pointsPerGame = double.Parse(worksheet.Cells[row, 3].GetValue<string>(), CultureInfo.InvariantCulture),
                minutesPerGame = double.Parse(worksheet.Cells[row, 4].GetValue<string>(), CultureInfo.InvariantCulture),
                reboundsPerGame = double.Parse(worksheet.Cells[row, 5].GetValue<string>(), CultureInfo.InvariantCulture),
                assistsPerGame = double.Parse(worksheet.Cells[row, 6].GetValue<string>(), CultureInfo.InvariantCulture),
            };
            NBAPlayers.Add(player);
        }
    }
    return NBAPlayers;
}

static void PopulateDatabase(List<NBAPlayer> players)
{
    using (var context = new NBAPlayerContext())
    {
        context.NBAPlayers.AddRange(players);
        context.SaveChanges();
    }
}

static void GetPlayers()
{
    using (var context = new NBAPlayerContext())
    {
        // Retrieve all NBA players from the database
        var allPlayers = context.NBAPlayers.ToList();

        // Display player information with abbreviated property names
        foreach (var player in allPlayers)
        {
            Console.WriteLine($"Name: {player.playerName}");
            Console.WriteLine($"GP: {player.gamesPlayed}");
            Console.WriteLine($"PPG: {player.pointsPerGame}");
            Console.WriteLine($"MPG: {player.minutesPerGame}");
            Console.WriteLine($"RPG: {player.reboundsPerGame}");
            Console.WriteLine($"APG: {player.assistsPerGame}");
            Console.WriteLine();
        }
    }
}