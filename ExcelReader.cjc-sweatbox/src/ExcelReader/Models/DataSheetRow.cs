namespace ExcelReader.Models;

/// <summary>
/// Represents a row in a data file.
/// </summary>
public class DataSheetRow
{
    #region Properties

    public int Id { get; set; }

    public int DataSheetId { get; set; }

    public int Position { get; set; }

    public List<DataItem> DataItems { get; set; } = [];

    public bool IsBlankRow => DataItems.Count == 0 || DataItems.All(x => string.IsNullOrWhiteSpace(x.Value));

    #endregion
    #region Methods

    public void Add(int position, string value)
    {
        Add(new DataItem
        {
            Position = position,
            Value = value
        });
    }

    public void Add(DataItem dataItem)
    {
        DataItems.Add(dataItem);
    }

    #endregion
}
