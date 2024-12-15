using ExcelReader.TwilightSaw.Model;

namespace ExcelReader.TwilightSaw.Reader;

public interface IReader
{
    ReaderItem Read();
}