namespace ExcelReader.kwm0304.Models;

public class Response<T>
{
  public int Id { get; set; }
  public string Header { get; set; }
  public List<string> ColumnNames { get; set; }

  public List<Dictionary<string, object>> RowValues { get; set; }

  public Response()
  {
    ColumnNames = [];
    RowValues = [];
    Header = "";
  }
  public Response(List<string> columnNames, List<Dictionary<string, object>> rows, string header)
  {
    ColumnNames = columnNames;
    RowValues = rows;
    Header = header;
  }
}