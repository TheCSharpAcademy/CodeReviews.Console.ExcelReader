using Spectre.Console;

namespace ExcelReader.kwm0304.Views;

public class UserInput
{
  public static string GetUserPath()
  {
    string folder = GetAndConfirm("What is the path to your csv?\n");
    // string file = GetAndConfirm("What is the file name: (including the .csv)");
    // string path = Path.Combine(folder, file);
    return folder;
  }
  public static int GetRowOptions(string question)
  {
    return AnsiConsole.Ask<int>($"{question}");
  }
  public static string GetAndConfirm(string question)
  {
    string folderPath = AnsiConsole.Ask<string>($"[bold blue]{question} [/]\n");
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