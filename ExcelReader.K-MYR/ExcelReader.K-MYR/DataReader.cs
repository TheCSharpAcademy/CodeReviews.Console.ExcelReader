using OfficeOpenXml;
using System.Data;


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
        var data = Repo.InsertDataWithOleDB(ws, file);

        return data;
    }
}
