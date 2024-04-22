namespace ExcelReader.Doc415;

internal class ImportExcelService
{
    public void ImportExcelToDb(string excelFile)
    {
        DbHandler _db = new();
        ExcelFileHandler _fileHandler = new ExcelFileHandler(excelFile);
        _db.PrepareDatabase();
        _db.CreateDBTable(_fileHandler.GetColumnNames());
        var rows = _fileHandler.GetRows();
        _db.InsertRowsToDb(rows);
    }

    public List<List<string>> GetDbData()
    {
        DbHandler _db = new();
        return MapQueryToView(_db.GetDbData());
    }

    private List<List<string>> MapQueryToView(IEnumerable<dynamic> rows)
    {
        List<List<string>> queryData = new();

        foreach (var row in rows)
        {
            List<string> rowValues = new();
            var propertyBag = (IDictionary<string, object>)row;
            foreach (var property in propertyBag)
            {
                rowValues.Add(property.Value.ToString());

            }
            queryData.Add(rowValues);
        }
        return queryData;
    }
}
