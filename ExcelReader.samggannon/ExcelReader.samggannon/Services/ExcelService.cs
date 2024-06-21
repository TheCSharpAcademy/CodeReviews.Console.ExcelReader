namespace ExcelReader.samggannon.Services;

internal class ExcelService
{
    private readonly FileInfo _fileName;

    public ExcelService()
    {

    }

    public void ReadExcelSheet(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
        {
            Console.WriteLine($"{fileName}: did not exist.");
            throw new FileNotFoundException("File not found");
        }

        Console.WriteLine("Reading data...");
    }
}
