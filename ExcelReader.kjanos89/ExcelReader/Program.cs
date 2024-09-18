using ExcelReader.Service;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Loading, please wait...");
        Service service = new Service();
        service.Initialize();
    }
}