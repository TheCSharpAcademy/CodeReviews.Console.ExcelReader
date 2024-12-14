using ExcelReader.TwilightSaw.Helper;
using ExcelReader.TwilightSaw.Service;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace ExcelReader.TwilightSaw.Controller;

public class ReaderController(IConfiguration configuration)
{
    public void Start()
    {
        while(true)
        {
            AnsiConsole.Write(new Rule("[olive]Supported formats - xlsx, csv, pdf, docx.[/]"));
            var input = UserInput.Create("Input the path of the file: ");
            if (!File.Exists(input))
            {
                Validation.EndMessage("File does not exist.");
                continue;
            }
            Console.Clear();
            var dbService = new DbService(configuration, new ReaderService(input));
            dbService.IsValid();
        }
       
    }
}