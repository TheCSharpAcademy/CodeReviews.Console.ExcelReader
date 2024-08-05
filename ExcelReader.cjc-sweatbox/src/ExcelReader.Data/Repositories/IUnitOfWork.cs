namespace ExcelReader.Data.Repositories;

/// <summary>
/// Definition of the Unit of Work pattern that must be implemented.
/// </summary>
public interface IUnitOfWork
{
    IDataFileRepository DataFiles { get; }
    IDataSheetRepository DataSheets { get; }
    IDatabaseRepository Database { get; }
    IDataFieldRepository DataFields { get; }
    IDataSheetRowRepository DataSheetRows { get; }
    IDataItemRepository DataItems { get; }
}
