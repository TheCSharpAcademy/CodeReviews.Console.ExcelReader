using ExcelReader.Database;
using ExcelReader.Models;

namespace ExcelReader.Services;

public class DataPersistenceService
{
    private ExcelReaderDbContext Db { get; set; }

    public DataPersistenceService(ExcelReaderDbContext db)
    {
        Db = db;
    }

    public void InsertParsedData(ParseResult parsedResult)
    {
        var cols = InsertCols(parsedResult.ParsedCols);
        var rows = InsertRows(parsedResult.ParsedRows);
        InsertCells(parsedResult.ParsedCells, rows, cols);
    }

    public List<Row> InsertRows(List<ParsedRowResult> parsedRows)
    {
        var rows = parsedRows.Select((parsedRow, i) =>
            new Row
            {
                RowIndex = i
            }
        ).ToList();

        Db.Rows.AddRange(rows);
        Db.SaveChanges();

        return rows;
    }

    public List<Col> InsertCols(List<ParsedColResult> parsedColResults)
    {
        var cols = parsedColResults.Select((parsedCol, i) =>
            new Col
            {
                ColIndex = i,
                Name = parsedCol.Name ?? "",
            }
        ).ToList();

        Db.Cols.AddRange(cols);
        Db.SaveChanges();

        return cols;
    }

    public List<Cell> InsertCells(List<ParsedCellResult> parsedCellResults, List<Row> rows, List<Col> cols)
    {
        var cells = parsedCellResults.Select((parsedCell, i) =>
            new Cell
            {
                ColId = cols[parsedCell.ColIndex].ColId,
                RowId = rows[parsedCell.RowIndex].RowId,
                Content = parsedCell.Content ?? "",
            }
        ).ToList();

        Db.Cells.AddRange(cells);
        Db.SaveChanges();

        return cells;
    }
}