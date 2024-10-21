using System.Data.SQLite;
using Dapper;
using Excel_Reader.Lawang.Model;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace Excel_Reader.Lawang.Data;

public class Database
{
    private readonly string connectionString;

    public Database(IConfiguration config)
    {
        connectionString = config.GetConnectionString("SqliteConnection") ?? "";
    }

    public void CreateDatabase()
    {
        try
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();
            string createTable = @"
                CREATE TABLE IF NOT EXISTS People(
                   Id INTEGER PRIMARY KEY AUTOINCREMENT,  
                   FirstName TEXT NOT NULL,
                   LastName TEXT NOT NULL,
                   Gender TEXT NOT NULL,
                   Country TEXT NOT NULL, 
                   Age INT NOT NULL,
                   Date TEXT NOT NULL
                );
            ";

            connection.Execute(createTable);
            connection.Close();

            AnsiConsole.MarkupLine("[green bold]DATABASE CREATED  :bookmark_tabs:[/]");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task InsertData(List<Person> people)
    {
        try
        {
            using var connection = new SQLiteConnection(connectionString);
            string insertTable = @"
                INSERT INTO People
                (FirstName, LastName, Gender, Country, Age, Date)
                VALUES(@firstName, @lastName, @gender, @country, @age, @date)
            ";

            for (int i = 0; i < people.Count(); i++)
            {
                var value = new
                {
                    @firstName = people[i].FirstName,
                    @lastName = people[i].LastName,
                    @gender = people[i].Gender,
                    @country = people[i].Country,
                    @age = people[i].Age,
                    @date = people[i].Date
                };

                await connection.ExecuteAsync(insertTable, value);
            }

            AnsiConsole.MarkupLine("[green bold]DATA SEEDED INTO THE DATABASE  	:books:[/]");


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task<List<Person>> GetAllData()
    {
        try
        {
            using var connection = new SQLiteConnection(connectionString);
            string getAllQuery = @"
                SELECT * FROM People; 
            ";

            var result = await connection.QueryAsync<Person>(getAllQuery);
            return result.ToList();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return new List<Person>();
    }
    public void CreateDynamicData(List<WorkSheet> workSheets)
    {
        try
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();
            foreach (WorkSheet ws in workSheets)
            {
                string createTable = @$"
                    CREATE TABLE IF NOT EXISTS {ws.Name}(
                        {string.Join(", ", ws.ColumnHeaders.Select(header => $"{header} TEXT"))} 
                    ) 
                ";
                connection.Execute(createTable);

                string insertTable = @$"
                        INSERT INTO {ws.Name}
                        ({string.Join(", ", ws.ColumnHeaders)})
                        VALUES{string.Join(", ", ws.TableValue)}; 
                    ";

                connection.Execute(insertTable);


            }

            connection.Close();
            AnsiConsole.MarkupLine("[green bold]DATA SEEDED INTO THE DATABASE  	:books:[/]");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


}
