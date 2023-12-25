using ExcelReader.UgniusFalze.Services;
using Microsoft.Extensions.Configuration;

try
{
    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();
    Display.DisplayStart();
    var driver = new Driver(config.GetConnectionString("LocalDb"));
    Display.DisplayDatabaseDrop();
    driver.DropDatabase();
    Display.DisplayDatabaseCreate();
    driver.CreateDatabase();
    Display.DisplayTableCreate();
    driver.CreateTable();
    Display.DisplayReadingFile();
    using var fileReader = new FileReader();
    var columns = fileReader.GetColumns();
    if (fileReader.GetColumns() == null)
    {
        Display.DisplayNotFound();
        return;
    }
    var planesFromExcel = fileReader.GetData();
    Display.DisplayWritingToDatabase();
    foreach (var planeInExcel in planesFromExcel)
    {
        driver.InsertPlane(planeInExcel);
    }
    var planes = driver.GetPlanes();
    Display.DisplayData(planes, columns);
    Display.DisplayDone();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

