using ExcelReader.TwilightSaw.Controller;
using ExcelReader.TwilightSaw.Factory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var app = HostFactory.CreateDbHost(args);

var scope = app.Services;
var configuration = scope.GetRequiredService<IConfiguration>();

var readerController = new ReaderController();

var dbFactory = new DbFactory(configuration, readerController);
dbFactory.CreateDb();
dbFactory.CreateTable();
