using ExcelReader.TwilightSaw.Controller;
using ExcelReader.TwilightSaw.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

    var app = HostFactory.CreateDbHost(args);

    var scope = app.Services;
    var configuration = scope.GetRequiredService<IConfiguration>();

    new ReaderController(configuration).Start();
    