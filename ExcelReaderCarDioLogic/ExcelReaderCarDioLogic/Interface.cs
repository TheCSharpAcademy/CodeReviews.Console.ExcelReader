namespace ExcelReaderCarDioLogic;

internal class Interface
{
    public string AskFilePath()
    {
        Console.WriteLine(@"Type the absolute path of your file");
        string filePath = Console.ReadLine();
        filePath = $@"{filePath}";

        return filePath;
    }

    public void ShowData(List<string> headers, List<List<string>> dataRows)
    {
        Console.WriteLine("Showing data in File...");
        Thread.Sleep(1000);

        List<List<string>> combinedList = new List<List<string>>();
        combinedList.Add(headers);
        combinedList.AddRange(dataRows);

        foreach (var row in combinedList)
        {
            Console.WriteLine(string.Join(", ", row));
        }

        Console.WriteLine("Data presented! Press any key to continue!");
        Console.ReadLine();
    }
}
