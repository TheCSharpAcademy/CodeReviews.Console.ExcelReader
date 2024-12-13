using ExcelReader.TwilightSaw.Factory;
using ExcelReader.TwilightSaw.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

    var app = HostFactory.CreateDbHost(args);

    var scope = app.Services;
    var configuration = scope.GetRequiredService<IConfiguration>();

    var readerService = new ReaderService();

    var db = new DbService(configuration, readerService);
    db.CreateDb();
    db.CreateTable();
    db.Read();
    
    readerService.ReadPdf();
