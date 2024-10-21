using System.ComponentModel.DataAnnotations;
using Excel_Reader.Lawang;
using Excel_Reader.Lawang.Data;
using Excel_Reader.Lawang.Model;
using Spectre.Console;

internal class App
{
    private readonly Database _db;
    private readonly ExcelOperator _excelOperator;
    private readonly FileReader _fileReader;

    public App(Database db, ExcelOperator excelOperator, FileReader fileReader)
    {
        _db = db;
        _excelOperator = excelOperator;
        this._fileReader = fileReader;
    }

    public async Task Run()
    {
        Console.Clear();
        View.RenderTitle("Excel-Reader", Color.RoyalBlue1, Color.DarkSeaGreen1, "Application", "skyblue3", BoxBorder.Heavy);
        string dbName = "Excel-Reader.db";

        //Delete the existing database if it exists
        DeleteDatabaseIfExists(dbName);

        //create Database
        AnsiConsole.Status().Start("Creating Database...", ctx =>
        {
            Thread.Sleep(1000);
            _db.CreateDatabase();
        });

        try
        {
            //Checks wether the presence of excel file
            FileInfo excelFile = Validation.GetFileInfo();
            if (!File.Exists(excelFile.FullName))
            {
                AnsiConsole.Markup("[red bold] EXCEL FILE DOES NOT EXIST[/]");
                return;
            }

            // Read from excel file, which returns list of person
            var peopleList = await AnsiConsole.Status().StartAsync("Reading from Excel...", async ctx =>
            {
                // To show what app is doing at current moment
                Thread.Sleep(1000);
                return await _excelOperator.ReadExcel(excelFile);
            });

            await AnsiConsole.Status().StartAsync("Seeding Data into database...", async ctx =>
            {
                // To show what app is doing at current moment
                Thread.Sleep(1000);
                await _db.InsertData(peopleList);
            });

            var listOfPerson = await _db.GetAllData();

            View.DisplayKnownExcel(listOfPerson);
            var confirmation = AnsiConsole.Prompt(
            new ConfirmationPrompt("[royalblue1 bold]\nDo you want to read another files (dynamically)?\n[/]"));


            //FROM THIS POINT CHALLENGE PROBLEM IS DEALT 
            if (confirmation) DynamicReader();

            Console.Clear();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
    private void DynamicReader()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            View.RenderTitle("File-Reader", Color.CadetBlue, Color.Gold1, "CHALLENGE-SECTION", "lime", BoxBorder.Rounded);

            string path = AnsiConsole.Ask<string>("[yellow bold]\n[grey](enter 'exit' to exit operation)[/]\nEnter the Path for your file : [/]");
            if(path.Equals("exit"))
            {
               return; 
            }
            if (!File.Exists(path))
            {
                AnsiConsole.MarkupLine("[red bold]File doesn't exist: [/]");
                var confirmation = AnsiConsole.Prompt(
                    new ConfirmationPrompt("[royalblue1 bold]\nDo you want to try again?\n[/]"));

                if (confirmation) continue;
                return;
            }
            FileInfo fileInfo = new FileInfo(path);

            switch(fileInfo.Extension)
            {
                case ".xlsx":
                    var operation = Validation.ValidateOperation();
                    Console.WriteLine();

                    if(operation.Equals("Read"))
                    {
                        DynamicExcel(fileInfo);
                    }
                    else if(operation.Equals("Write"))
                    {
                        _excelOperator.WriteIntoFile(fileInfo);
                    }
                break;

                case ".csv":
                    _fileReader.ReadCSV(fileInfo);
                break;

                case ".pdf":
                    _fileReader.ReadPdf(fileInfo);
                break;

                case ".doc":
                    _fileReader.ReadDOC(fileInfo);
                break;
            }



            exit = AnsiConsole.Prompt(
                new ConfirmationPrompt("[royalblue1 bold]\nDo you want to exit Application?\n[/]")
            );
        }
    }

    private void DynamicExcel(FileInfo fileInfo)
    {
        try
        {
            Validation.ValidateExcel(fileInfo);

            List<WorkSheet> workSheets = AnsiConsole.Status().Start("Reading Excel File Dynamically... ", ctx =>
            {
                Thread.Sleep(1000);
                return _excelOperator.ReadDynamicExcel(fileInfo);
            });

            AnsiConsole.Status().Start("Seending into Database...", ctx =>
            {
                Thread.Sleep(1000);
                _db.CreateDynamicData(workSheets);
            });
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red bold]{ex.Message}[/]");
        }
    }
    private void DeleteDatabaseIfExists(string dbName)
    {

        if (File.Exists(dbName))
        {
            File.Delete(dbName);
        }
    }


}