namespace ExcelReader.Configurations;

/// <summary>
/// Class to hold the required options for the Application.
/// </summary>
public class ApplicationOptions
{
    #region Properties

    public string DatabaseName { get; set; } = "";

    public string DatabaseExtension { get; set; } = "";

    public string WorkingDirectoryPath { get; set; } = "";

    #endregion
}
