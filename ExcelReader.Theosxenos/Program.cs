using ExcelReader.Controller;
using ExcelReader.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = new HostApplicationBuilder();

builder.Services.AddSingleton<DapperContext>();

builder.Services.AddTransient<MainController>();

var host = builder.Build();

host.Services.GetRequiredService<MainController>().Start();

