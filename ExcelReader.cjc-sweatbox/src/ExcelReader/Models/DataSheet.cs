namespace ExcelReader.Models;

/// <summary>
/// Represents a worksheet in a data file.
/// </summary>
public class DataSheet
{
    #region Properties

    public int Id { get; set; }

    public int DataFileId { get; set; }

    public int Position { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<DataField> DataFields { get; set; } = [];

    public List<DataSheetRow> DataSheetRows { get; set; } = [];

    #endregion
}
