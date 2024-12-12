using ExcelReader.jollejonas.Models;
using OfficeOpenXml;

namespace ExcelReader.jollejonas.Services;

public class ImportData
{
    private readonly UserService _database;

    public ImportData(UserService database)
    {
        _database = database;
    }
    public void ImportXlsxData()
    {
        string filePath = $"{Directory.GetCurrentDirectory()}/Data.xlsx";
        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            var worksheet = package.Workbook.Worksheets["Ark1"];

            var users = GetList<User>(worksheet);

            Console.WriteLine("Creating database...");
            foreach (var user in users)
            {
                _database.AddUser(user);
            }
        }

    }

    public List<T> GetList<T>(ExcelWorksheet worksheet)
    {
        Console.WriteLine("Reading from excel...");
        List<T> list = new List<T>();
        var columnInfo = Enumerable.Range(1, worksheet.Dimension.Columns).ToList().Select(n =>
            new { Index = n, ColumnName = worksheet.Cells[1, n].Value.ToString() }
            );

        for (int row = 1; row < worksheet.Dimension.Rows; row++)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            foreach (var prop in typeof(T).GetProperties())
            {
                int column = columnInfo.SingleOrDefault(c => c.ColumnName == prop.Name).Index;
                var value = worksheet.Cells[row, column].Value;
                var propType = prop.PropertyType;
                prop.SetValue(obj, Convert.ChangeType(value, propType));
            }

            list.Add(obj);
        }

        return list;
    }


}
