using ExcelReader.Database;
using OfficeOpenXml;

namespace ExcelReader.Service
{
    public class Service
    {
        public static string filePath; //add the filepath
        private Context context;
        public Service()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            context = new Context();
        }

        public void DataFlow()
        {
            RemoveDB();
            CreateDB();
            RemoveDB();
            DisplayData();
        }

        private void RemoveDB()
        {
            try
            {
                context.Database.EnsureDeleted();
                Console.WriteLine("Database removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CreateDB()
        {
            try
            {
                context.Database.EnsureCreated();
                Console.WriteLine("Database has been created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DisplayData()
        {

        }

        private void ReadData()
        {

        }
    }
}