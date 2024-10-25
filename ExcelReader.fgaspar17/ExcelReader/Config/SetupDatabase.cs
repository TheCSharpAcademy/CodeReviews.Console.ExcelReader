using Microsoft.Data.Sqlite;

namespace ExcelReader;

public class SetupDatabase
{
    public static void ResetDatabase(string file)
    {
        FileInfo fi = new(file);
        try
        {
            if (fi.Exists)
            {
                SqliteConnection connection = new(GlobalConfig.ConnectionString);
                connection.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                fi.Delete();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            fi.Delete();
        }
    }

    public static bool CreateTable(string name, List<string> columns)
    {
        string columnsFormatted = string.Join(", \n", columns.Select(c => $"\"{c}\" TEXT NULL").ToList());

        try
        {
            using SqliteConnection connection = new(GlobalConfig.ConnectionString);

            connection.Open();

            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText = @$"CREATE TABLE IF NOT EXISTS ""{name}"" (
	                        {columnsFormatted}
                            );";
            cmd.ExecuteNonQuery();

            connection.Close();

        }
        catch (Exception ex)
        {

            Console.WriteLine($"An error ocurred: {ex.Message}");
            return false;
        }
        return true;
    }
}