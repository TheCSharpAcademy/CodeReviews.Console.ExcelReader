using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ExcelReader.Controllers;

namespace ExcelReader;

public class ExcelReader
{
    public static void Main()
    {
        IHost? app;
        try
        {
            app = StartUp.AppInit();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Thread.Sleep(4000);
            return;
        }
        app.Services.CreateScope()
            .ServiceProvider.GetRequiredService<DataController>().Start();
    }
}