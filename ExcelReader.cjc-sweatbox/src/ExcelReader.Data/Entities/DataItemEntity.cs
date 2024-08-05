using ExcelReader.Models;
using SQLite;

namespace ExcelReader.Data.Entities;

/// <summary>
/// Database version of the DataItem object.
/// </summary>
[Table("DataItem")]
public class DataItemEntity
{
    #region Properties

    [PrimaryKey]
    [NotNull]
    [AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public int DataFieldId { get; set; }

    [NotNull]
    public int DataSheetRowId { get; set; }

    [NotNull]
    public int Position { get; set; }

    [NotNull]
    public string Value { get; set; } = string.Empty;

    #endregion
    #region Methods

    public static DataItemEntity MapFrom(DataItem model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        ArgumentNullException.ThrowIfNull(model.Value, nameof(model.Value));

        return new DataItemEntity
        {
            Id = model.Id,
            DataFieldId = model.DataFieldId,
            DataSheetRowId = model.DataSheetRowId,
            Position = model.Position,
            Value = model.Value,
        };
    }

    public static DataItem MapTo(DataItemEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        ArgumentNullException.ThrowIfNull(entity.Value, nameof(entity.Value));

        return new DataItem
        {
            Id = entity.Id,
            DataFieldId = entity.DataFieldId,
            DataSheetRowId = entity.DataSheetRowId,
            Position = entity.Position,
            Value = entity.Value,
        };
    }

    #endregion
}
