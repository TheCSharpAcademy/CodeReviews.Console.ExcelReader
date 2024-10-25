using Spectre.Console;

namespace ExcelReader;

public static class PathValidator
{
    public static bool ExistsExcelFilePath(string path)
    {
        if (!IsValidExcelFilePath(path)) return false;

        System.IO.FileInfo fi = null;
        try
        {
            fi = new System.IO.FileInfo(path);
            if (!fi.Exists) return false;
        }
        catch (ArgumentException ex) { AnsiConsole.WriteException(ex); }
        catch (System.IO.PathTooLongException ex) { AnsiConsole.WriteException(ex); }
        catch (NotSupportedException ex) { AnsiConsole.WriteException(ex); }
        if (ReferenceEquals(fi, null))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool IsValidExcelFilePath(string path)
    {
        if (!IsValidFilePath(path)) return false;

        if (Path.GetExtension(path).StartsWith(".xls"))
            return true;
        else
            return false;
    }

    public static bool IsValidFilePath(string path)
    {
        System.IO.FileInfo fi = null;
        try
        {
            fi = new System.IO.FileInfo(path);
        }
        catch (ArgumentException ex) { AnsiConsole.WriteException(ex); }
        catch (System.IO.PathTooLongException ex) { AnsiConsole.WriteException(ex); }
        catch (NotSupportedException ex) { AnsiConsole.WriteException(ex); }
        if (ReferenceEquals(fi, null))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}