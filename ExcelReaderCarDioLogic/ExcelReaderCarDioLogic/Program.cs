using ExcelReaderCarDioLogic;

FileReaderLogic logic = new FileReaderLogic();
DatabaseLogic model = new DatabaseLogic();
Interface inter = new Interface();

string filePath = inter.AskFilePath();
(List<string> headers, List<List<string>> dataRows) = logic.ReadExcel(filePath);


if (headers != null)
{
    inter.ShowData(headers, dataRows);

    try
    {
        model.DeleteDatabase();
    }
    catch { }

    model.CreateDatabase();

    model.CreateTableAddData(headers, dataRows);
}