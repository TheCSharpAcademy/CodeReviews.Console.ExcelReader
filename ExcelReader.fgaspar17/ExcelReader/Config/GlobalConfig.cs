namespace ExcelReader;

public static class GlobalConfig
{
    public static string? ConnectionString { get; set; }
    public static string? FilePath { get; set; }

    public static void InitializeConnectionString(string? connectionString)
    {
        ConnectionString = connectionString;
    }
}
