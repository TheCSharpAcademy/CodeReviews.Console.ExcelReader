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
            var inputRead = UserInput.Create("Input the path of the file: ");
            if (!File.Exists(inputRead))
            {
                Validation.EndMessage("File does not exist.");
                continue;
            }
            Console.Clear();
            var readerService = new ReaderService(inputRead);
            var dbService = new DbService(configuration, readerService);
            dbService.IsValid();
            var inputConfirm = UserInput.CreateWithConfirm("Do you want to edit this file?");
            if (inputConfirm)
            {
                AnsiConsole.Write(new Rule("[olive]First row - table columns, separation: xlsx, csv - space; docx, pdf - coma and space.[/]"));
                var inputInsert = UserInput.Create("Your text: ");
                readerService.Write(inputInsert);
            }
            Validation.EndMessage("");
        }
    }
}