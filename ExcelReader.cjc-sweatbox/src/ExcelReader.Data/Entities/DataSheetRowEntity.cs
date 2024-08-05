using ExcelReader.Models;
using SQLite;

namespace ExcelReader.Data.Entities;

/// <summary>
/// Database version of the DataSheetRow object.
/// </summary>
[Table("DataSheetRow")]
public class DataSheetRowEntity
{
    #region Properties

    [PrimaryKey]
    [NotNull]
    [AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public int DataSheetId { get; set; }

    [NotNull]
    public int Position { get; set; }

    #endregion
    #region Methods

    public static DataSheetRowEntity MapFrom(DataSheetRow model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        return new DataSheetRowEntity
        {
            Id = model.Id,
            DataSheetId = model.DataSheetId,
            Position = model.Position,
        };
    }

    public static DataSheetRow MapTo(DataSheetRowEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        return new DataSheetRow
        {
            Id = entity.Id,
            DataSheetId = entity.DataSheetId,
            Position = entity.Position,
        };
    }

    #endregion
}
