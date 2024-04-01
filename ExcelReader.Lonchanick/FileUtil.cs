
namespace EPPlusSamples;

public class FileUtil
{
    static DirectoryInfo _outputDir = null;
    public static DirectoryInfo OutputDir
    {
        get
        {
            return _outputDir;
        }
        set
        {
            _outputDir = value;
            if (!_outputDir.Exists)
            {
                _outputDir.Create();
            }
        }
    }
    public static FileInfo GetCleanFileInfo(string file)
    {
        var fi = new FileInfo(OutputDir.FullName + Path.DirectorySeparatorChar + file);
        if (fi.Exists)
        {
            fi.Delete();  // ensures we create a new workbook
        }
        return fi;
    }
    public static FileInfo GetFileInfo(string file)
    {
        return new FileInfo(OutputDir.FullName + Path.DirectorySeparatorChar + file);
    }

    public static FileInfo GetFileInfo(DirectoryInfo altOutputDir, string file, bool deleteIfExists = true)
    {
        var fi = new FileInfo(altOutputDir.FullName + Path.DirectorySeparatorChar + file);
        if (deleteIfExists && fi.Exists)
        {
            fi.Delete();  // ensures we create a new workbook
        }
        return fi;
    }


    internal static DirectoryInfo GetDirectoryInfo(string directory)
    {
        var di = new DirectoryInfo(_outputDir.FullName + Path.DirectorySeparatorChar + directory);
        if (!di.Exists)
        {
            di.Create();
        }
        return di;
    }
    /// <summary>
    /// Returns a fileinfo with the full path of the requested file
    /// </summary>
    /// <param name="directory">A subdirectory</param>
    /// <param name="file"></param>
    /// <returns></returns>
    public static FileInfo GetFileInfo(string directory, string file)
    {
        var rootDir = GetRootDirectory().FullName;
        return new FileInfo(Path.Combine(rootDir, directory, file));
    }

    public static DirectoryInfo GetRootDirectory()
    {
        var currentDir = AppDomain.CurrentDomain.BaseDirectory;
        while (!currentDir.EndsWith("bin"))
        {
            currentDir = Directory.GetParent(currentDir).FullName.TrimEnd('\\');
        }
        return new DirectoryInfo(currentDir).Parent;
    }

    public static DirectoryInfo GetSubDirectory(string directory, string subDirectory)
    {
        var currentDir = GetRootDirectory().FullName;
        return new DirectoryInfo(Path.Combine(currentDir, directory, subDirectory));
    }
}