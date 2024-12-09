using ExcelReader.TwilightSaw.Controller;
using ExcelReader.TwilightSaw.Factory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var app = HostFactory.CreateDbHost(args);

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
context.Database.Migrate();