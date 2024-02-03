using Spectre.Console;

namespace ExcelReader.Doc415;

internal class FileHandler
{
    public static string SelectFile()
    {
        string selectedFile = "";
        string directory = AppDomain.CurrentDomain.BaseDirectory;
        List<string> files = Directory.GetFiles(directory).ToList();
        for (int i = files.Count - 1; i >= 0; i--)
        {
            var splited = files[i].Split('.');
            var extension = splited[splited.Length - 1];
            if (extension != "xlsx")
                files.RemoveAt(i);
        }

        selectedFile = AnsiConsole.Prompt(new SelectionPrompt<string>().
                                                   Title("Select excel file to import").
                                                   AddChoices(files)
                                                   );
        return selectedFile;
    }
}
