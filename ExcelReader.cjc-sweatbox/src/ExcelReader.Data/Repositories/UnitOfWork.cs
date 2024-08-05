namespace ExcelReader.Data.Repositories;

/// <summary>
/// Unit of Work pattern implementation.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    #region Constructors

    public UnitOfWork(
        IDatabaseRepository databaseRepository,
        IDataFileRepository dataFileRepository,
        IDataSheetRepository dataSheetRepository,
        IDataFieldRepository dataFieldRepository,
        IDataSheetRowRepository dataSheetRowRepository,
        IDataItemRepository dataItemRepository)
    {
        Database = databaseRepository;
        DataFiles = dataFileRepository;
        DataSheets = dataSheetRepository;
        DataFields = dataFieldRepository;
        DataSheetRows = dataSheetRowRepository;
        DataItems = dataItemRepository;
    }

    #endregion
    #region Properties

    public IDatabaseRepository Database { get; }

    public IDataFileRepository DataFiles { get; }

    public IDataSheetRepository DataSheets { get; }

    public IDataFieldRepository DataFields { get; }

    public IDataSheetRowRepository DataSheetRows { get; }

    public IDataItemRepository DataItems { get; }

    #endregion
}
