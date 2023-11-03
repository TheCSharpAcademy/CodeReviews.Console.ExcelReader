using ExcelReader.Models;

namespace ExcelReader
{
    public class ExcelService
    {
        public ExcelContext _context;

        public ExcelService(ExcelContext context)
        {
            _context = context;
        }

        public void Create(List<ExcelModel> models)
        {
            try
            {
                _context.Excels.AddRange(models);
                _context.SaveChanges();
                Console.WriteLine("Data is added to database");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured while adding data to database :" + ex.Message);
            }
        }
    }
}
