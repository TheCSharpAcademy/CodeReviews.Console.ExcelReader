using System.Data.SQLite;

public class Player
{
    public int Rk { get; set; }
    public string Pos { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public int G { get; set; }
    public int PA { get; set; }
    public int AB { get; set; }
    public int R { get; set; }
    public int H { get; set; }
    public int Doubles { get; set; }
    public int Triples { get; set; }
    public int HR { get; set; }
    public int Rbi { get; set; }
    public int SB { get; set; }
    public int CS { get; set; }
    public int BB { get; set; }
    public int SO { get; set; }
    public decimal BA { get; set; }
    public decimal Obp { get; set; }
    public decimal Slg { get; set; }
    public decimal Ops { get; set; }
    public int OpsPlus { get; set; }
    public int TB { get; set; }
    public int Gdp { get; set; }
    public int Hbp { get; set; }
    public int SH { get; set; }
    public int SF { get; set; }
    public int Ibb { get; set; }
}

public class DatabaseManager
{
    private const string DatabaseFileName = "databaseExcelTest.db";
    private const string ConnectionString = "Data Source=" + DatabaseFileName;

    public void CreateDatabase()
    {
        if (File.Exists(DatabaseFileName))
            File.Delete(DatabaseFileName);

        using (var connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "CREATE TABLE Players (Rk INTEGER, Pos TEXT, Name TEXT, Age INTEGER, G INTEGER, PA INTEGER, AB INTEGER, R INTEGER, H INTEGER, Doubles INTEGER, Triples INTEGER, HR INTEGER, RBI INTEGER, SB INTEGER, CS INTEGER, BB INTEGER, SO INTEGER, BA DECIMAL(10,3), OBP DECIMAL(10,3), SLG DECIMAL(10,3), OPS DECIMAL(10,3), OPSPlus INTEGER, TB INTEGER, GDP INTEGER, HBP INTEGER, SH INTEGER, SF INTEGER, IBB INTEGER)";
                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Database created.");
    }

    public void PopulateDatabase(List<Player> players)
    {
        using (var connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();

            foreach (var player in players)
            {
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO Players (Rk, Pos, Name, Age, G, PA, AB, R, H, Doubles, Triples, HR, RBI, SB, CS, BB, SO, BA, OBP, SLG, OPS, OPSPlus, TB, GDP, HBP, SH, SF, IBB) VALUES (@Rk, @Pos, @Name, @Age, @G, @PA, @AB, @R, @H, @Doubles, @Triples, @HR, @RBI, @SB, @CS, @BB, @SO, @BA, @OBP, @SLG, @OPS, @OPSPlus, @TB, @GDP, @HBP, @SH, @SF, @IBB)";
                    command.Parameters.AddWithValue("@Rk", player.Rk);
                    command.Parameters.AddWithValue("@Pos", player.Pos);
                    command.Parameters.AddWithValue("@Name", player.Name);
                    command.Parameters.AddWithValue("@Age", player.Age);
                    command.Parameters.AddWithValue("@G", player.G);
                    command.Parameters.AddWithValue("@PA", player.PA);
                    command.Parameters.AddWithValue("@AB", player.AB);
                    command.Parameters.AddWithValue("@R", player.R);
                    command.Parameters.AddWithValue("@H", player.H);
                    command.Parameters.AddWithValue("@Doubles", player.Doubles);
                    command.Parameters.AddWithValue("@Triples", player.Triples);
                    command.Parameters.AddWithValue("@HR", player.HR);
                    command.Parameters.AddWithValue("@RBI", player.Rbi);
                    command.Parameters.AddWithValue("@SB", player.SB);
                    command.Parameters.AddWithValue("@CS", player.CS);
                    command.Parameters.AddWithValue("@BB", player.BB);
                    command.Parameters.AddWithValue("@SO", player.SO);
                    command.Parameters.AddWithValue("@BA", player.BA);
                    command.Parameters.AddWithValue("@OBP", player.Obp);
                    command.Parameters.AddWithValue("@SLG", player.Slg);
                    command.Parameters.AddWithValue("@OPS", player.Ops);
                    command.Parameters.AddWithValue("@OPSPlus", player.OpsPlus);
                    command.Parameters.AddWithValue("@TB", player.TB);
                    command.Parameters.AddWithValue("@GDP", player.Gdp);
                    command.Parameters.AddWithValue("@HBP", player.Hbp);
                    command.Parameters.AddWithValue("@SH", player.SH);
                    command.Parameters.AddWithValue("@SF", player.SF);
                    command.Parameters.AddWithValue("@IBB", player.Ibb);
                    command.ExecuteNonQuery();
                }
            }
        }

        Console.WriteLine("Database populated.");
    }

    public List<Player> FetchData()
    {
        var players = new List<Player>();

        using (var connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "SELECT Rk, Pos, Name, Age, G, PA, AB, R, H, Doubles, Triples, HR, RBI, SB, CS, BB, SO, BA, OBP, SLG, OPS, OPSPlus, TB, GDP, HBP, SH, SF, IBB FROM Players";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var player = new Player
                        {
                            Rk = reader.GetInt32(0),
                            Pos = reader.GetString(1),
                            Name = reader.GetString(2),
                            Age = reader.GetInt32(3),
                            G = reader.GetInt32(4),
                            PA = reader.GetInt32(5),
                            AB = reader.GetInt32(6),
                            R = reader.GetInt32(7),
                            H = reader.GetInt32(8),
                            Doubles = reader.GetInt32(9),
                            Triples = reader.GetInt32(10),
                            HR = reader.GetInt32(11),
                            Rbi = reader.GetInt32(12),
                            SB = reader.GetInt32(13),
                            CS = reader.GetInt32(14),
                            BB = reader.GetInt32(15),
                            SO = reader.GetInt32(16),
                            BA = reader.GetDecimal(17),
                            Obp = reader.GetDecimal(18),
                            Slg = reader.GetDecimal(19),
                            Ops = reader.GetDecimal(20),
                            OpsPlus = reader.GetInt32(21),
                            TB = reader.GetInt32(22),
                            Gdp = reader.GetInt32(23),
                            Hbp = reader.GetInt32(24),
                            SH = reader.GetInt32(25),
                            SF = reader.GetInt32(26),
                            Ibb = reader.GetInt32(27)
                        };

                        players.Add(player);
                    }
                }
            }
        }

        return players;
    }
}

