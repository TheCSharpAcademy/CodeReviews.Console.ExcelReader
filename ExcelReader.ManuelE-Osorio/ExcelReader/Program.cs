﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ExcelReader.Controllers;
using ExcelReader.UI;

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
            MainUI.ErrorMessage(e.Message);
            Thread.Sleep(3000);
            return;
        }
        app.Services.CreateScope()
            .ServiceProvider.GetRequiredService<DataController>().Start();
    }
}