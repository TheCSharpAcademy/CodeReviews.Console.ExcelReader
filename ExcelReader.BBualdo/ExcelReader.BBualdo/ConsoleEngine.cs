using ExcelReader.BBualdo.Models;
using Spectre.Console;

namespace ExcelReader.BBualdo;

public static class ConsoleEngine
{
  public static void GetPeopleTable(List<Person> people)
  {
    if (people.Count == 0)
    {
      AnsiConsole.Markup("[red]List is empty.[/]");
      return;
    }

    Table table = new Table().Title("People List");
    TableColumn[] columns = [new("ID"), new("First Name"), new("Last Name"), new("Age"), new("Country")];
    table.AddColumns(columns);

    foreach (Person person in people)
    {
      table.AddRow(person.Id.ToString(), person.FirstName ?? "", person.LastName ?? "", person.Age.ToString(), person.Country ?? "");
    }

    AnsiConsole.Write(table);
  }
}