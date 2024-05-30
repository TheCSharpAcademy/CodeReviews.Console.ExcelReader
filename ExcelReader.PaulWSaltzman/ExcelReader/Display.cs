using ExcelReader.Models;
using ExcelReader.Services;
using Spectre.Console;
using System.Globalization;

namespace ExcelReader
{
    internal class Display
    {
        private readonly CheckService _checkService;

        public Display(CheckService checkService)
        {
            _checkService = checkService;
        }


        public async Task RunProgramAsync()
        {
            List<Check> checksFromDataBase = new List<Check>();
            List<Check> checks = new List<Check>();

            string excelPath = Directory.GetCurrentDirectory();
            excelPath = excelPath + "\\IssuedChecks.xlsx";


            await AnsiConsole.Status()
                .StartAsync("Starting Program...", async ctx =>
                {
                    //check file location
                    if (!Excel.ExcelFileExists(excelPath))
                        {
                            AnsiConsole.MarkupLine($@"LOG: Excel file not found.  Please check ExcelReader\Excelreader\IssuedChecks.xlsx, and reload the program");
                            Console.ReadKey();
                            Environment.Exit(0);
                        };

                    AnsiConsole.MarkupLine("LOG: Excel File Found ...");

                    AnsiConsole.MarkupLine("LOG: Checking for Database ...");
                    ctx.Status("Checking for old Database");
                    ctx.Spinner(Spinner.Known.Dots12);
                    ctx.SpinnerStyle(Style.Parse("green"));
                    if (!await _checkService.CheckDatabase())
                    {
                        AnsiConsole.MarkupLine("LOG: Database not found skipping Database removal ...");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("LOG: Database found preparing to burn ...");
                        ctx.Status("Lighting Flame Thrower");
                        //drop old database
                        AnsiConsole.MarkupLine("LOG: Burning old database ...");
                        ctx.Status("Burning Database");
                        await _checkService.DropDatabaseAsync();
                        AnsiConsole.MarkupLine("LOG: Database Burned ...");
                        ctx.Status("Extinguishing the fire");
                        ctx.Spinner(Spinner.Known.Dots12);
                        ctx.SpinnerStyle(Style.Parse("green"));
                    }

                    //create database
                    ctx.Status("Fire extinguished Building database...");
                    AnsiConsole.MarkupLine("LOG: Building a new database ...");
                    await _checkService.CreateDatabaseAsync();
                    ctx.Status("database created");
                    ctx.Spinner(Spinner.Known.Dots12);
                    ctx.SpinnerStyle(Style.Parse("green"));

                    //Load Excel fle
                    AnsiConsole.MarkupLine("LOG: Loading excel file ...");
                    checks = Excel.LoadChecks(excelPath);
                    ctx.Status("excel file loaded");
                    ctx.Spinner(Spinner.Known.Dots12);
                    ctx.SpinnerStyle(Style.Parse("green"));

                    //load checks into database
                    AnsiConsole.MarkupLine("LOG: Loading checks into database ...");
                    await _checkService.InsertChecks(checks);
                    ctx.Status("inserting checks");
                    ctx.Spinner(Spinner.Known.Dots12);
                    ctx.SpinnerStyle(Style.Parse("green"));

                    AnsiConsole.MarkupLine($"LOG: Inserted {checks.Count} checks successfully.");
                    ctx.Status("checks inserted");
                    ctx.Spinner(Spinner.Known.Dots12);
                    ctx.SpinnerStyle(Style.Parse("green"));

                    //Load checks back from the database
                    AnsiConsole.MarkupLine($"LOG: Pulling checks from database...");
                    checksFromDataBase = await _checkService.GetAllChecks();
                    ctx.Status("checks retrieved");
                    ctx.Spinner(Spinner.Known.Dots12);
                    ctx.SpinnerStyle(Style.Parse("green"));
                });
            // write data to screen
            await ShowChecks(checksFromDataBase);
        }

        public async Task ShowChecks(List<Check> checks)
        {
            var table = new Table();


            table.AddColumn("ID");
            table.AddColumn("Account Number");
            table.AddColumn("Check Number");
            table.AddColumn("Amount");
            table.AddColumn("Date");
            table.AddColumn("Canceled");

            table.Columns[0].RightAligned();
            table.Columns[3].RightAligned();

            foreach (var check in checks)
            {
                string formattedAmount = check.Amount.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));

                table.AddRow($@"{check.CheckKid}",
                            $@"{check.AccountNo}",
                            $@"{check.CheckNo}",
                            $@"{formattedAmount}",
                            $@"{check.Date}",
                            $@"{check.Canceled}");
            }
            AnsiConsole.Write(table);
            Console.ReadLine();
        }
    }
}
