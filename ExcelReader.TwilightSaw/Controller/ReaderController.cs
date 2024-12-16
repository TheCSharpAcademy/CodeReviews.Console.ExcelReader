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
            AnsiConsole.Write(new Rule("[olive]Supported formats - xlsx, csv.[/]"));
            var inputRead = UserInput.Create("Input the path of the file: ");
            if (!File.Exists(inputRead))
            {
                Validation.EndMessage("File does not exist.");
                continue;
            }
            Console.Clear();
            var readerService = new ReaderService(inputRead);
            var dbService = new DbService(configuration, readerService);
            var isValid = dbService.IsValid();
            if(!isValid) {
                Validation.EndMessage("Bad file format or some sheets are empty.");
                continue;
            }
            var inputConfirm = UserInput.CreateWithConfirm("Do you want to edit this file?");
            if (inputConfirm)
            {
                Console.Clear();
                readerService.Write();
            }
            Validation.EndMessage("");
        }
    }
}