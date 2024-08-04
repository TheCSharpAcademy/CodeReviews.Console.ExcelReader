using Spectre.Console;

namespace ExcelReader.kwm0304.Views;

public class UserInput
{
  public static FileInfo GetUserPath()
  {
    string folder = GetAndConfirm("Please provide the path to the folder your file is in:");
    string file = GetAndConfirm("What is the file name: (including the .csv)");
    string path = Path.Combine(folder, file);
    return new FileInfo(path);
  }
  public static int GetRowOptions(string question)
  {
    return AnsiConsole.Ask<int>($"{question}");
  }
  public static string GetAndConfirm(string question)
  {
    string folderPath = AnsiConsole.Ask<string>($"[bold chartreuse_1]{question} [/]\n");
    if (string.IsNullOrEmpty(folderPath) || string.IsNullOrWhiteSpace(folderPath))
    {
      return GetAndConfirm(question);
    }
    if (AnsiConsole.Confirm(folderPath))
    {
      return folderPath;
    }
    return GetAndConfirm(question);
  }
}