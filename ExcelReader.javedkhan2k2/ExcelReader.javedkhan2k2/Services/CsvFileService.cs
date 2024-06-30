using ExcelReader.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace ExcelReader.Services;

public class CsvFileService
{
    private readonly string _filePath;
    private ExcelPackage _package;
    private ExcelWorksheet _worksheet;

    public CsvFileService(string filePath)
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
            var format = new ExcelTextFormat
            {
                SkipLinesEnd = 1,
                Delimiter = ';'
            };
            var ts = TableStyles.Dark1;
            Console.WriteLine("Reading the file.");
            FileInfo existingFile = new FileInfo(_filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _package = new ExcelPackage();
            _worksheet = _package.Workbook.Worksheets.Add("Sheet1");
            _worksheet.Cells["A1"].LoadFromText(existingFile, format, ts, FirstRowIsHeader: true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the Csv file: {ex.Message}");
        }

    }

    internal List<Contact> ExtractContactList()
    {
        List<Contact> contacts = new List<Contact>();
        try
        {
            int totalRows = _worksheet.Dimension.Rows;
            for (int row = 2; row <= totalRows; row++)
            {
                contacts.Add(ExtractContact(_worksheet, row));
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the Csv file: {ex.Message}");
        }
        Console.WriteLine("File is successfully read");
        return contacts;
    }

    private Contact ExtractContact(ExcelWorksheet worksheet, int row)
    {
        Contact contact = new Contact();
        contact.Name = worksheet.Cells[row, 1].Text;
        contact.Email = worksheet.Cells[row, 2].Text;
        contact.PhoneNumber = worksheet.Cells[row, 3].Text;
        contact.Address = worksheet.Cells[row, 4].Text;
        return contact;
    }

    internal void ExportAsExcel()
    {
        try
        {
            if (!Directory.Exists("output"))
            {
                Directory.CreateDirectory("output");
            }
            using (ExcelPackage package = new ExcelPackage())
            {
                package.Workbook.Worksheets.Add("Sheet1", _worksheet);
                FileInfo outputFile = new FileInfo(@$"output/CsvToExcel{DateTime.Now.ToLongTimeString()}.xlsx");
                package.SaveAs(outputFile);
            }
            Console.WriteLine("File is successfully exported as Excel.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while converting to CSV: {ex.Message}");
        }
    }

    internal void ExportAsPdf()
    {
        try
        {
            var outputFileName = "CsvToPdf" + DateTime.Now.ToLongTimeString();
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

    internal void DisposeWorksheet()
    {
        if(_package != null)
        {
            _package.Dispose();
            Console.WriteLine("ExcelPackage disposed.");
        }
    }

}