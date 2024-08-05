using ExcelReader.Data.Repositories;
using ExcelReader.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExcelReader.ConsoleApp.Installers;

/// <summary>
/// Microsoft.Extensions.DependencyInjection.IServiceCollection interface extension methods.
/// </summary>
public static class IServiceCollectionExtension
{
    #region Methods

    public static void RegisterServices(this IServiceCollection services)
    {
        // App.
        services.AddHostedService<App>();

        // Data.
        services.AddSingleton(typeof(IEntityRepository<>), typeof(SqliteEntityRepository<>));
        services.AddScoped<IDatabaseRepository, SqliteDatabaseRepository>();
        services.AddScoped<IDataFileRepository, DataFileRepository>();
        services.AddScoped<IDataSheetRepository, DataSheetRepository>();
        services.AddScoped<IDataFieldRepository, DataFieldRepository>();
        services.AddScoped<IDataSheetRowRepository, DataSheetRowRepository>();
        services.AddScoped<IDataItemRepository, DataItemRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Service.
        services.AddScoped<IDataFileProcessor, DataFileProcessor>();
        services.AddScoped<IDataFileReader, DataFileReader>();
        services.AddScoped<ICsvDataFileReader, CsvDataFileReader>();
        services.AddScoped<IExcelDataFileReader, ExcelDataFileReader>();
        services.AddScoped<IDataManager, DataManager>();
    }

    #endregion
}
