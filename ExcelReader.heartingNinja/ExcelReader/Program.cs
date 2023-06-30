using OfficeOpenXml;

class Program
{
    static void Main()
    {
        var filePath = "your location for where the file is\\dataGiantsBaseballHitting.xlsx";

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var players = ReadFromExcel(filePath);

        var databaseManager = new DatabaseManager();
        databaseManager.CreateDatabase();
        databaseManager.PopulateDatabase(players);

        var fetchedPlayers = databaseManager.FetchData();

        Console.WriteLine($"Rk\tPos\t{"Name".PadRight(25)}\tAge\tG\tPA\tAB\tR\tH\tDoubles\tTriples\tHR\tRBI\tSB\tCS\tBB\tSO\tBA\tOBP\tSLG\tOPS\tOPSPlus\tTB\tGDP\tHBP\tSH\tSF\tIBB");

        foreach (var player in fetchedPlayers)
        {
            Console.WriteLine($"{player.Rk}\t{player.Pos}\t{player.Name.PadRight(25)}\t{player.Age}\t{player.G}\t{player.PA}\t{player.AB}\t{player.R}\t{player.H}\t{player.Doubles}\t{player.Triples}\t{player.HR}\t{player.Rbi}\t{player.SB}\t{player.CS}\t{player.BB}\t{player.SO}\t{player.BA}\t{player.Obp}\t{player.Slg}\t{player.Ops}\t{player.OpsPlus}\t{player.TB}\t{player.Gdp}\t{player.Hbp}\t{player.SH}\t{player.SF}\t{player.Ibb}");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static List<Player> ReadFromExcel(string filePath)
    {
        var players = new List<Player>();

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0];

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var player = new Player
                {
                    Rk = GetIntValue(worksheet.Cells[row, 1]),
                    Pos = GetStringCellValue(worksheet.Cells[row, 2]),
                    Name = GetStringCellValue(worksheet.Cells[row, 3]),
                    Age = GetIntValue(worksheet.Cells[row, 4]),
                    G = GetIntValue(worksheet.Cells[row, 5]),
                    PA = GetIntValue(worksheet.Cells[row, 6]),
                    AB = GetIntValue(worksheet.Cells[row, 7]),
                    R = GetIntValue(worksheet.Cells[row, 8]),
                    H = GetIntValue(worksheet.Cells[row, 9]),
                    Doubles = GetIntValue(worksheet.Cells[row, 10]),
                    Triples = GetIntValue(worksheet.Cells[row, 11]),
                    HR = GetIntValue(worksheet.Cells[row, 12]),
                    Rbi = GetIntValue(worksheet.Cells[row, 13]),
                    SB = GetIntValue(worksheet.Cells[row, 14]),
                    CS = GetIntValue(worksheet.Cells[row, 15]),
                    BB = GetIntValue(worksheet.Cells[row, 16]),
                    SO = GetIntValue(worksheet.Cells[row, 17]),
                    BA = GetDecimalValue(worksheet.Cells[row, 18]),
                    Obp = GetDecimalValue(worksheet.Cells[row, 19]),
                    Slg = GetDecimalValue(worksheet.Cells[row, 20]),
                    Ops = GetDecimalValue(worksheet.Cells[row, 21]),
                    OpsPlus = GetIntValue(worksheet.Cells[row, 22]),
                    TB = GetIntValue(worksheet.Cells[row, 23]),
                    Gdp = GetIntValue(worksheet.Cells[row, 24]),
                    Hbp = GetIntValue(worksheet.Cells[row, 25]),
                    SH = GetIntValue(worksheet.Cells[row, 26]),
                    SF = GetIntValue(worksheet.Cells[row, 27]),
                    Ibb = GetIntValue(worksheet.Cells[row, 28])
                };

                players.Add(player);
            }
        }

        return players;
    }

    static int GetIntValue(ExcelRangeBase cell)
    {
        return int.TryParse(cell?.Value?.ToString(), out int value) ? value : 0;
    }

    static decimal GetDecimalValue(ExcelRangeBase cell)
    {
        return decimal.TryParse(cell?.Value?.ToString(), out decimal value) ? value : 0;
    }

    static string GetStringCellValue(ExcelRangeBase cell)
    {
        return cell?.Value?.ToString() ?? string.Empty;
    }
}




