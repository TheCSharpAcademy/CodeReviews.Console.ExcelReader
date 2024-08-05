using ExcelReader.Models;
using SQLite;

namespace ExcelReader.Data.Entities;

/// <summary>
/// Database version of the DataSheet object.
/// </summary>
[Table("DataSheet")]
public class DataSheetEntity
{
    #region Properties

    [PrimaryKey]
    [NotNull]
    [AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public int DataFileId { get; set; }

    [NotNull]
    public int Position { get; set; }

    [NotNull]
    public string Name { get; set; } = string.Empty;

    #endregion
    #region Methods

    public static DataSheetEntity MapFrom(DataSheet model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        ArgumentException.ThrowIfNullOrWhiteSpace(model.Name, nameof(model.Name));

        return new DataSheetEntity
        {
            Id = model.Id,
            DataFileId = model.DataFileId,
            Position = model.Position,
            Name = model.Name,
        };
    }

    public static DataSheet MapTo(DataSheetEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        ArgumentException.ThrowIfNullOrWhiteSpace(entity.Name, nameof(entity.Name));

        return new DataSheet
        {
            Id = entity.Id,
            DataFileId = entity.DataFileId,
            Position = entity.Position,
            Name = entity.Name,
        };
    }

    #endregion
}
