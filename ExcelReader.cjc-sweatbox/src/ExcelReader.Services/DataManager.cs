using ExcelReader.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace ExcelReader.Services;

/// <summary>
/// Partial DataManager class for non-entity specific data access methods.
/// </summary>
public partial class DataManager : IDataManager
{
    #region Fields

    private readonly ILogger<DataManager> _logger;
    private readonly IUnitOfWork _unitOfWork;

    #endregion
    #region Constructors

    public DataManager(ILogger<DataManager> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    #endregion
    #region Methods

    public bool ResetDatabase()
    {
        _logger.LogInformation("Starting {method}", nameof(ResetDatabase));
        _unitOfWork.Database.EnsureDeleted();
        _unitOfWork.Database.EnsureCreated();
        _logger.LogInformation("Finished {method}", nameof(ResetDatabase));

        return true;
    }

    #endregion
}
