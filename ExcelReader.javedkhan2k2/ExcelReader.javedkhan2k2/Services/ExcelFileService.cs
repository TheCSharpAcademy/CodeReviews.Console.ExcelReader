using ExcelReader.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;

namespace ExcelReader.Services;

public class ExcelFileService
{
    private readonly string _filePath;
    private ExcelPackage _package;
    private ExcelWorksheet _worksheet;

    public ExcelFileService(string filePath)
    {
        _filePath = filePath;
        ReadWorksheet();
    }

    private void ReadWorksheet()
    {
        Console.WriteLine($"Checking if {_filePath} exists.");
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException($"The file {_filePath} does not exist.");
        }
        Console.WriteLine($"{_filePath} verification completed.");
        try
        {
            Console.WriteLine("Reading the file.");
            FileInfo existingFile = new FileInfo(_filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage _package = new ExcelPackage(existingFile);
            _worksheet =  _package.Workbook.Worksheets[0];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the Excel file: {ex.Message}");
        }

    }

    public List<Contact> ExtractContacts()
    {
        List<Contact> contacts = new List<Contact>();
        try
        {
            Console.WriteLine("Converting the file to Objects");
            int totalRows = _worksheet.Dimension.Rows;
            for (int row = 2; row <= totalRows; row++)
            {
                contacts.Add(ExtractContact(row));
            }
            Console.WriteLine("File is successfully read");
            return contacts;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the Excel file: {ex.Message}");
            return null;
        }

    }

    internal void ExportAsCSV()
    {
        try
        {
            var format = new ExcelOutputTextFormat
            {
                Delimiter = ';'
            };
            int lastRow = _worksheet.Dimension.End.Row;
            var outputFileName = "ExcelToCSV" + DateTime.Now.ToLongTimeString();
            if (!Directory.Exists("output"))
            {
                Directory.CreateDirectory("output");
            }
            var outputFile = new FileInfo(@$"output/{outputFileName}.csv");
            _worksheet.Cells[$"A1:D{lastRow}"].SaveToText(outputFile, format);
            Console.WriteLine("File is successfully exported as CSV.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while converting to CSV: {ex.Message}");
        }
    }

    internal void ExportAsPDF()
    {
        try
        {
            int lastRow = _worksheet.Dimension.End.Row;
            var outputFileName = "ExcelToPdf" + DateTime.Now.ToLongTimeString();
            if (!Directory.Exists("output"))
            {
                Directory.CreateDirectory("output");
            }
            var outputFile = $"output/{outputFileName}.pdf";
            Document document = new Document(PageSize.A4);
            PdfWriter.GetInstance(document, new FileStream(@$"{outputFile}", FileMode.Create));

            document.Open();

            PdfPTable table = new PdfPTable(_worksheet.Dimension.End.Column);

            for (int col = 1; col <= _worksheet.Dimension.End.Column; col++)
            {
                table.AddCell(new Phrase(_worksheet.Cells[1, col].Text));
            }

            for (int row = 2; row <= _worksheet.Dimension.End.Row; row++)
            {
                for (int col = 1; col <= _worksheet.Dimension.End.Column; col++)
                {
                    table.AddCell(new Phrase(_worksheet.Cells[row, col].Text));
                }
            }

            document.Add(table);

            document.Close();
            Console.WriteLine("PDF file created successfully.");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while exporting the file to PDF: {ex.Message}");
        }
    }

    private Contact ExtractContact(int row)
    {
        Contact contact = new Contact();
        contact.Name = _worksheet.Cells[row, 1].Text;
        contact.Email = _worksheet.Cells[row, 2].Text;
        contact.PhoneNumber = _worksheet.Cells[row, 3].Text;
        contact.Address = _worksheet.Cells[row, 4].Text;
        return contact;
    }

    internal void DisposeWorksheet()
    {
        if(_package != null)
        {
            _package.Dispose();
            Console.WriteLine("ExcelPackage disposed.");
        }
        
    }
    
}