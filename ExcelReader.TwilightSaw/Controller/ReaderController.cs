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
            AnsiConsole.Write(new Rule("[lightyellow3]Welcome to the ExcelReader![/]"));
            AnsiConsole.Write(new Rule("[olive]Read formats - xlsx, csv, docx, pdf; Write formats - xlsx.[/]"));
            AnsiConsole.Write(new Rule("[gold3]Format - csv: coma, docx: coma and space, pdf: coma and space.[/]"));
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
            if(Path.GetExtension(inputRead).ToLower() == ".xlsx")
            {
                var inputConfirm = UserInput.CreateWithConfirm("Do you want to edit this file?");
                if (inputConfirm)
                {
                    Console.Clear();
                    readerService.Write();
                }
            }
            Validation.EndMessage("");
        }
    }
}