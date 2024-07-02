using ExcelReader.Database;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace ExcelReader.Services;

public class SetupService
{
    private ExcelReaderDbContext Db;

    public SetupService(ExcelReaderDbContext db)
    {
        Db = db;
    }

    public void Setup()
    {
        AnsiConsole.MarkupLine("\n[bold][green]Initialising Excel Reader ...[/][/]");

        AnsiConsole.MarkupLine("\n[blue]Deleting database...[/]");
        AnsiConsole.MarkupLine("\tEnsuring database is safe to delete...");
        AnsiConsole.MarkupLine("\tDetaching entities...");
        Db.ChangeTracker
            .Entries()
            .ToList()
            .ForEach(e => e.State = EntityState.Detached);

        Db.Database.EnsureDeleted();
        AnsiConsole.MarkupLine("\t[green]Done[/]");

        AnsiConsole.MarkupLine("\n[blue]Initialise fresh database...[/]");
        AnsiConsole.MarkupLine("\tCreating database...");
        AnsiConsole.MarkupLine("\tCreating database schema...");
        Db.Database.EnsureCreated();
        AnsiConsole.MarkupLine("\t[green]Done[/]");
    }
}