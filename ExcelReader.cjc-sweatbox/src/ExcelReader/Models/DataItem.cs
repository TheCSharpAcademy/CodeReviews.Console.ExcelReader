namespace ExcelReader.Models;

/// <summary>
/// Represents a cell / value in a data file.
/// </summary>
public class DataItem
{
    #region Properties

    public int Id { get; set; }

    public int DataFieldId { get; set; }

    public int DataSheetRowId { get; set; }

    public int Position { get; set; }

    public string Value { get; set; } = string.Empty;

    #endregion
}
