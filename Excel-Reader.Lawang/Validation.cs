using Spectre.Console;

namespace Excel_Reader.Lawang;

public static class Validation
{
    public static FileInfo GetFileInfo()
    {
        string dir = Directory.GetCurrentDirectory();
        string path = Path.Combine(dir, "sample.xlsx");

        FileInfo fileInfo = new FileInfo(path);
        if (fileInfo.Extension == ".xls")
        {
            // checks whether the Excel file is in the older Excel 97-2003 format (.xls), which is an OLE compound document. Can't read by epplus
            throw new Exception("The EPPlus library only supports the newer Excel formats (.xlsx, .xlsm, .xltx, .xltm) that are based on the Open XML standard.");
        }
        return fileInfo;
    }

    public static void ValidateExcel(FileInfo fileInfo)
    {
        if (fileInfo.Extension == ".xls")
        {
            throw new Exception("The EPPlus library only supports the newer Excel formats (.xlsx, .xlsm, .xltx, .xltm) that are based on the Open XML standard.");
        }
    }

    public static string ValidateOperation()
    {
        View.ShowInstruction();
        var operation = AnsiConsole.Prompt(
            new TextPrompt<string>("[yellow bold]Select your operation?[/]")
                .AddChoices(["Read", "Write", "Exit"])
                .DefaultValue("Exit"));

        return operation;
    }
}
