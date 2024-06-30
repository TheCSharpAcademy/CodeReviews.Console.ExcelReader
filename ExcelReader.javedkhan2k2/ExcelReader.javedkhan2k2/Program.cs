using DocumentFormat.OpenXml.Office.CustomUI;
using ExcelReader;
using ExcelReader.Data;
using ExcelReader.Repositories;
using ExcelReader.Services;
using Exercisetacker.UI;
using Spectre.Console;


var menu = new ExcelReaderMenu();
bool runApplication = true;
while (runApplication)
{
    AnsiConsole.Clear();
    var choice = menu.GetMainMenu();
    switch (choice)
    {
        case "Read Excel File":
            ReadExcelFile();
            break;
        case "Read CSV file":
            ReadCsvFile();
            break;
        case "Read Doc File":
            ReadDocFile();
            break;
        case "Exit":
            runApplication = false;
            break;
        default:
            break;
    }
}

void ReadDocFile()
{
    try
    {
        using (var context = new ContactsDbContext())
        {
            Console.WriteLine("Deleting the previous Database. Please Wait......");
            context.Database.EnsureDeleted();
            Console.WriteLine("Database Deletion completed.");
            Console.WriteLine("Creating new Database.");
            context.Database.EnsureCreated();
            Console.WriteLine("Database Creation Completed.");
        }
        var filePath = "input.docx";
        WordFileService reader = new WordFileService(filePath);
        Console.WriteLine("Contact Extracting From Word Document started.");
        var contacts = reader.ExtractContacts();
        Console.WriteLine("Contact Extracting finished.");
        Console.WriteLine("Saving Contacts to Database.");
        var contactRepository = new ContactRepository(new ContactsDbContext());
        contactRepository.AddBulkContacts(contacts);
        Console.WriteLine("Saving Contacts to Database is completed.");

        var contactsFromDb = contactRepository.GetAllContacts();
        VisualizationEngine.DisplayAllContacts(contactsFromDb, "Displaying All Contacts From Database");
        VisualizationEngine.DisplayContinueMessage();

    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occuered.\nError: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"InnerException: \n{ex.InnerException}");
        }
    }
}

void ReadCsvFile()
{
    try
    {
        using (var context = new ContactsDbContext())
        {
            Console.WriteLine("Deleting the previous Database. Please Wait......");
            context.Database.EnsureDeleted();
            Console.WriteLine("Database Deletion completed.");
            Console.WriteLine("Creating new Database.");
            context.Database.EnsureCreated();
            Console.WriteLine("Database Creation Completed.");
        }
        var filePath = "input.csv";
        CsvFileService reader = new CsvFileService(filePath);
        Console.WriteLine("Contact Extracting started.");
        var contacts = reader.ExtractContactList();
        Console.WriteLine("Contact Extracting finished.");
        Console.WriteLine("Saving Contacts to Database.");
        var contactRepository = new ContactRepository(new ContactsDbContext());
        contactRepository.AddBulkContacts(contacts);
        Console.WriteLine("Saving Contacts to Database is completed.");

        var contactsFromDb = contactRepository.GetAllContacts();
        VisualizationEngine.DisplayAllContacts(contactsFromDb, "Displaying All Contacts From Database");
        while (true)
        {
            var choice = menu.GetCsvMenu();
            switch (choice)
            {
                case "Export as Excel":
                    reader.ExportAsExcel();
                    break;
                case "Export as PDF":
                    reader.ExportAsPdf();
                    break;
                case "Exit": 
                    reader.DisposeWorksheet();
                    return;
                default:
                    break;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occuered.\nError: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"InnerException: \n{ex.InnerException}");
        }
    }
}

void ReadExcelFile()
{
    try
    {
        using (var context = new ContactsDbContext())
        {
            Console.WriteLine("Deleting the previous Database. Please Wait......");
            context.Database.EnsureDeleted();
            Console.WriteLine("Database Deletion completed.");
            Console.WriteLine("Creating new Database.");
            context.Database.EnsureCreated();
            Console.WriteLine("Database Creation Completed.");
        }
        var filePath = "input.xlsx";
        ExcelFileService reader = new ExcelFileService(filePath);
        Console.WriteLine("Contact Extracting started.");
        var contacts = reader.ExtractContacts();
        Console.WriteLine("Contact Extracting finished.");
        Console.WriteLine("Saving Contacts to Database.");
        var contactRepository = new ContactRepository(new ContactsDbContext());
        contactRepository.AddBulkContacts(contacts);
        Console.WriteLine("Saving Contacts to Database is completed.");

        var contactsFromDb = contactRepository.GetAllContacts();
        VisualizationEngine.DisplayAllContacts(contactsFromDb, "Displaying All Contacts From Database");
        while (true)
        {
            var choice = menu.GetExcelMenu();
            switch (choice)
            {
                case "Export as CSV":
                    reader.ExportAsCSV();
                    break;
                case "Export as PDF":
                    reader.ExportAsPdf();
                    break;
                case "Exit": 
                    reader.DisposeWorksheet();
                    return;
                default:
                    break;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occuered.\nError: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"InnerException: \n{ex.InnerException}");
        }
    }
}