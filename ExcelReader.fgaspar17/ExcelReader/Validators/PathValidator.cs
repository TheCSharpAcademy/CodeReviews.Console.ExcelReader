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
        catch (ArgumentException) { }
        catch (System.IO.PathTooLongException) { }
        catch (NotSupportedException) { }
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
        catch (ArgumentException) { }
        catch (System.IO.PathTooLongException) { }
        catch (NotSupportedException) { }
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