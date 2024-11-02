using ExcelReader;
using OfficeOpenXml;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

Startup.StartApplication();

ExcelProcessor excelProcessor = new ExcelProcessor(GlobalConfig.FilePath!);
excelProcessor.Run();