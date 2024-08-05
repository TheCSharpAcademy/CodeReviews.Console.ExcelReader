namespace ExcelReader.Models;

/// <summary>
/// Represents a column / field header in a data file.
/// </summary>
public class DataField
{
    #region Properties

    public int Id { get; set; }

    public int DataSheetId { get; set; }

    public int Position { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<DataItem> DataItems { get; set; } = [];

    #endregion
}
