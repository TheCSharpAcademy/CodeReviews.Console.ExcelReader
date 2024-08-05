using System.Data.SQLite;
using ExcelReader.Configurations;
using Microsoft.Extensions.Options;

namespace ExcelReader.Data.Repositories;

/// <summary>
/// Sqlite database repository methods.
/// </summary>
public class SqliteDatabaseRepository : IDatabaseRepository
{
    #region Fields

    private readonly ApplicationOptions _options;
    private readonly IDataFileRepository _dataFileRepository;
    private readonly IDataSheetRepository _dataSheetRepository;
    private readonly IDataFieldRepository _dataFieldRepository;
    private readonly IDataSheetRowRepository _dataSheetRowRepository;
    private readonly IDataItemRepository _dataItemRepository;

    #endregion
    #region Constructors

    public SqliteDatabaseRepository(
        IOptions<ApplicationOptions> options,
        IDataFileRepository dataFileRepository,
        IDataSheetRepository dataSheetRepository,
        IDataFieldRepository dataFieldRepository,
        IDataSheetRowRepository dataSheetRowRepository,
        IDataItemRepository dataItemRepository)
    {
        _options = options.Value;
        _dataFileRepository = dataFileRepository;
        _dataSheetRepository = dataSheetRepository;
        _dataFieldRepository = dataFieldRepository;
        _dataSheetRowRepository = dataSheetRowRepository;
        _dataItemRepository = dataItemRepository;
    }

    #endregion
    #region Properties

    public string ConnectionString => $"Data Source={FileName}";

    private string FileName => Path.ChangeExtension(_options.DatabaseName, _options.DatabaseExtension);

    private string FilePath => Path.GetFullPath(FileName);

    #endregion
    #region Methods

    public void EnsureCreated()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(FileName));

        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        connection.Close();

        _dataFileRepository.CreateTable();
        _dataSheetRepository.CreateTable();
        _dataFieldRepository.CreateTable();
        _dataSheetRowRepository.CreateTable();
        _dataItemRepository.CreateTable();
    }

    public void EnsureDeleted()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(FileName));

        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }
    }

    #endregion
}
