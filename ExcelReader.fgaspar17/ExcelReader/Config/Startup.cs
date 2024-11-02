using Microsoft.Extensions.Configuration;

namespace ExcelReader;

public class Startup
{
    public static void StartApplication()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfiguration config = builder.Build();

        string connectionString = config.GetConnectionString("Sqlite")
            ?? throw new InvalidOperationException("You must provide a Sqlite Connection String in the appsettings.json file.");
        GlobalConfig.InitializeConnectionString(connectionString);

        string dbFile = config.GetValue<string>("DatabaseFileName")
            ?? throw new InvalidOperationException("You must provide a DatabaseFileName in the appsettings.json file.");

        string filePath = config.GetValue<string>("ExcelFilePath")
            ?? throw new InvalidOperationException("You must provide a ExcelFilePath in the appsettings.json file.");
        if (!PathValidator.ExistsExcelFilePath(filePath))
            throw new InvalidOperationException("You must provide a valid path in the appsettings.json file.");

        GlobalConfig.FilePath = filePath;

        SetupDatabase.ResetDatabase(dbFile);
    }
}
