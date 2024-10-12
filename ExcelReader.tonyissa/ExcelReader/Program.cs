using ExcelReader.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<FileProcesser>();


var host = builder.Build();