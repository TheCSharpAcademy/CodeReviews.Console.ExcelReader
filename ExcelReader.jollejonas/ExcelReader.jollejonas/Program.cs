using ExcelReader.jollejonas.Data;
using ExcelReader.jollejonas.Services;

namespace ExcelReader.jollejonas
{
    class Program
    {
        static void Main(string[] args)
        {
            var excelReaderContext = new ExcelReaderContext();
            var userService = new UserService(excelReaderContext);
            var importData = new ImportData(userService);

            userService.RecreateDatabase();
            importData.ImportXlsxData();
            userService.GetUsers();
        }
    }
}
