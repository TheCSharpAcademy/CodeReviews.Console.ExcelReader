using ExcelReader.Models;
using SQLite;

namespace ExcelReader.Data.Entities;

/// <summary>
/// Database version of the DataFile object.
/// </summary>
[Table("DataFile")]
public class DataFileEntity
{
    #region Properties

    [PrimaryKey]
    [NotNull]
    [AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Name { get; set; } = string.Empty;

    [NotNull]
    public string Extension { get; set; } = string.Empty;

    [NotNull]
    public long Size { get; set; }

    #endregion
    #region Methods

    public static DataFileEntity MapFrom(DataFile model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        ArgumentException.ThrowIfNullOrWhiteSpace(model.Name, nameof(model.Name));
        ArgumentException.ThrowIfNullOrWhiteSpace(model.Extension, nameof(model.Extension));

        return new DataFileEntity
        {
            Id = model.Id,
            Name = model.Name,
            Extension = model.Extension,
            Size = model.Size
        };
    }

    public static DataFile MapTo(DataFileEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        ArgumentException.ThrowIfNullOrWhiteSpace(entity.Name, nameof(entity.Name));
        ArgumentException.ThrowIfNullOrWhiteSpace(entity.Extension, nameof(entity.Extension));

        return new DataFile
        {
            Id = entity.Id,
            Name = entity.Name,
            Extension = entity.Extension,
            Size = entity.Size
        };
    }

    #endregion
}
