using System.Data.SQLite;

namespace Kmakai.ExcelReader;

public class DataBase
{
    private readonly string ConnectionString = @"Data Source=Students_Database.db";
  

    public void CreateDatabase()
    {
        Console.WriteLine("Creating database...");

        if(File.Exists("Students_Database.db"))
        {
            File.Delete("Students_Database.db");
        }

        SQLiteConnection.CreateFile("Students_Database.db");

        if (!File.Exists("Students_Database.db"))
        {
            throw new Exception("Database file not created!");
        }

        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        var command = new SQLiteCommand("CREATE TABLE Students (Id INTEGER PRIMARY KEY, Name TEXT, Surname TEXT, Major TEXT)", connection);
        command.ExecuteNonQuery();

        connection.Close();

        Console.WriteLine("Database and Table created!");
    }

    public void InsertData(StudentInfo student)
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        var command = new SQLiteCommand("INSERT INTO Students (Name, Surname, Major) VALUES (@Name, @Surname, @Major)", connection);
        command.Parameters.AddWithValue("@Name", student.Name);
        command.Parameters.AddWithValue("@Surname", student.Surname);
        command.Parameters.AddWithValue("@Major", student.Major);

        command.ExecuteNonQuery();

        connection.Close();
    }

    public List<StudentInfo> GetStudents()
    {
        Console.WriteLine("Getting students from database...");

        var students = new List<StudentInfo>();

        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        var command = new SQLiteCommand("SELECT * FROM Students", connection);
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var student = new StudentInfo
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString(),
                Surname = reader["Surname"].ToString(),
                Major = reader["Major"].ToString()
            };

            students.Add(student);
        }

        connection.Close();

        Console.WriteLine("Students retrieved from database!");

        return students;
    }
}
