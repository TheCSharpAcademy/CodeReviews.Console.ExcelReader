using ExcelReader.Models;
using ExcelReader.Repositories.Interfaces;
using OfficeOpenXml;
using Spectre.Console;

namespace ExcelReader;

internal class Services
{
    string[] tableHeader = new string[3];

    private readonly IPersonRepository _personRepository;

    public Services(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public (string[], string[]) ReadExcel()
    {
        UserInterface.Loading("Reading Table...");

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using ExcelPackage package = new ExcelPackage(new FileInfo(@"C:\Users\pedro\OneDrive\Área de Trabalho\Journey\ExcelReader\ExcelReader\test.xlsx"));

        ExcelWorksheet workSheet = package.Workbook.Worksheets.FirstOrDefault();
        int rows = workSheet.Dimension.Rows;
        int columns = workSheet.Dimension.Columns;

        string[] personData = new string[3];
        for (int i = 2; i <= rows; i++)
        {
            for (int j = 1; j <= columns; j++)
            {
                personData[j - 1] = workSheet.Cells[i, j].Value.ToString();
            }
            FetchData(personData);

        }

        tableHeader[0] = workSheet.Cells[1, 1].Value.ToString();
        tableHeader[1] = workSheet.Cells[1, 2].Value.ToString();
        tableHeader[2] = workSheet.Cells[1, 3].Value.ToString();

        return (tableHeader, personData);
    }

    public void FetchData(string[] personData)
    {
        Person person = new Person()
        {
            Name = personData[0],
            Age = int.Parse(personData[1]),
            Job = personData[2],
        };

        _personRepository.AddPerson(person);
    }

    public void DropTable()
    {
        _personRepository.DropTable();
    }

    public void PrintTable()
    {
        UserInterface.Loading("Printing Table...");
        Table table = new Table();

        table.AddColumn(tableHeader[0]);
        table.AddColumn(tableHeader[1]);
        table.AddColumn(tableHeader[2]);

        List<Person> persons = _personRepository.GetAll();
        foreach (Person person in persons)
        {
            table.AddRow(person.Name, person.Age.ToString(), person.Job);
        }
        AnsiConsole.Write(table);
        Console.ReadKey();
    }

}
