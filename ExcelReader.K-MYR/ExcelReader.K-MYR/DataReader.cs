using OfficeOpenXml;
using System.Data;
using System.Runtime.InteropServices;


namespace ExcelReader.K_MYR;

internal class DataReader
{
    public readonly SQLServerRepo Repo;

    public DataReader(SQLServerRepo repo)
    {
        Repo = repo;
    }

    public DataTable? ReadFromXlsx(FileInfo file)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var package = new ExcelPackage(file);

        var ws = package.Workbook.Worksheets[0];

        Repo.CreateDatabase();
        Repo.CreateTable(ws);

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Repo.InsertDataWithOleDB(ws, file);
        }
        else
        {
            return Repo.InsertData(ws);
        }
    }
}
