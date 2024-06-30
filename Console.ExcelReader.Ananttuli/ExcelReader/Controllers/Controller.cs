using ExcelReader.Database;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace ExcelReader.Controllers;

public class Controller
{
    private ExcelReaderDbContext Db { get; set; }

    public Controller(ExcelReaderDbContext db)
    {
        Db = db;
    }

    public (List<List<string>> rows, List<string> cols) FetchSerializedData()
    {
        var rows = new List<List<string>>();

        var cellGroups = Db.Cells
            .Include(c => c.Row)
            .Include(c => c.Col)
            .GroupBy(c => c.RowId)
            .ToList();

        if (cellGroups.Count == 0)
        {
            return (rows, Db.Cols.Select(c => c.Name ?? "").ToList());
        }

        var cols = cellGroups[0].Select((rowCell) => $"{rowCell.Col.Name}").ToList();

        for (int i = 0; i < cellGroups.Count; i++)
        {
            var group = cellGroups[i];
            var cells = group.Select((cell) => cell.Content?.ToString() ?? "").ToList();

            rows.Add(cells);
        }

        return (rows, cols);
    }
}