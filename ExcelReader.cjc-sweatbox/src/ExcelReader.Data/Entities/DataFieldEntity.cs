using ExcelReader.Models;
using SQLite;

namespace ExcelReader.Data.Entities;

/// <summary>
/// Database version of the DataField object.
/// </summary>
[Table("DataField")]
public class DataFieldEntity
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

    [NotNull]
    public string Name { get; set; } = string.Empty;

    #endregion
    #region Methods

    public static DataFieldEntity MapFrom(DataField model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        ArgumentNullException.ThrowIfNull(model.Name, nameof(model.Name));

        return new DataFieldEntity
        {
            Id = model.Id,
            DataSheetId = model.DataSheetId,
            Position = model.Position,
            Name = model.Name,
        };
    }

    public static DataField MapTo(DataFieldEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        ArgumentNullException.ThrowIfNull(entity.Name, nameof(entity.Name));

        return new DataField
        {
            Id = entity.Id,
            DataSheetId = entity.DataSheetId,
            Position = entity.Position,
            Name = entity.Name,
        };
    }

    #endregion
}
