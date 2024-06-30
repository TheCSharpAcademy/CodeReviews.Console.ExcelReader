using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ExcelReader.Entities;

namespace ExcelReader.Services;

public class WordFileService
{
    private readonly string _filePath;

    public WordFileService(string filePath)
    {
        _filePath = filePath;
    }

    internal List<Contact> ExtractContacts()
    {
        List<Contact> contacts = new List<Contact>();
        Console.WriteLine($"Checking if {_filePath} exists.");
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException($"The file {_filePath} does not exist.");
        }
        Console.WriteLine($"{_filePath} verification completed.");
        try
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(_filePath, false))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;
                var tables = body.Elements<Table>();
                bool isHeader = true;
                foreach (var table in tables)
                {
                    foreach (var row in table.Elements<TableRow>())
                    {
                        if (!isHeader)
                        {
                            contacts.Add(ExtractContact(row));
                        }
                        if (isHeader)
                        {
                            isHeader = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the Excel file: {ex.Message}");
        }
        Console.WriteLine("File is successfully read");
        return contacts;
    }
    private Contact ExtractContact(TableRow row)
    {
        Contact contact = new Contact();
        var cells = row.Elements<TableCell>().ToList();
        contact.Name = cells[0].InnerText;
        contact.Email = cells[1].InnerText;
        contact.PhoneNumber = cells[2].InnerText;
        contact.Address = cells[3].InnerText;
        return contact;
    }

}