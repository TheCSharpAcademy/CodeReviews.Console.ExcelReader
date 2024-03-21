namespace ExcelReader.Doc415
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var importer = new ImportControl();
            importer.Import();
        }
    }
}
