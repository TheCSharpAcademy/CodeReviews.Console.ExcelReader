// See https://aka.ms/new-console-template for more information
using ExcelReader.samggannon.Controllers;
using ExcelReader.samggannon.Data;
using ExcelReader.samggannon.Services;
using ExcelReader.samggannon.UI;
using Microsoft.Extensions.DependencyInjection;

MainAsync(args).GetAwaiter().GetResult();

Console.WriteLine("Press [enter] to cease teasting.");
Console.ReadLine();

static async Task MainAsync(string[] args)
{

    var serviceProvider = new ServiceCollection()
    .AddSingleton<PlayerContext>()
    .AddSingleton<IPlayerService, PlayerService>()
    .AddScoped<DbPlayerConroller>()
    .BuildServiceProvider();

    await ConsoleOutput.EnsuredatabaseDeletion(serviceProvider);
}


// Requirements/ToDo
//
// This is an application that will read data from an Excel spreadsheet into a database
// When the application starts, it should delete the database if it exists, create a new one, create all tables, read from Excel, seed into the database.
// You need to use EPPlus package
// You shouldn't read into Json first.
// You can use SQLite or SQL Server (or MySQL if you're using a Mac)
// Once the database is populated, you'll fetch data from it and show it in the console.
// You don't need any user input
// You should print messages to the console letting the user know what the app is doing at that moment (i.e. reading from excel; creating tables, etc)
// The application will be written for a known table, you don't need to make it dynamic.
// When submitting the project for review, you need to include an xls file that can be read by your application.

// Additonal
// Before anything else you’ll have to create an Excel table that will be stored in your main project folder.
// The more organised the easier it will be for your program to read it. The first row of your columns need to be the property names of your model class
// Remember, this time you don’t need any user input. The only interaction your program will have with the user is to show the data from your database.
// You could structure the program in three parts. One for database creation, one for reading from the file and return a list and the last to populate your database using the returned list
