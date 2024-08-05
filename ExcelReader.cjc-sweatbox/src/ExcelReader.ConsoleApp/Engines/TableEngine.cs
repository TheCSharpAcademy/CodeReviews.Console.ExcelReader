using ExcelReader.Models;
using Spectre.Console;

namespace ExcelReader.ConsoleApp.Engines;

/// <summary>
/// Engine for Spectre.Table generation.
/// </summary>
internal class TableEngine
{
    #region Methods

    internal static Table GetTable(string title, DataSheet worksheet)
    {
        var table = new Table
        {
            Title = new TableTitle(title),
            Caption = new TableTitle($"{worksheet.DataSheetRows.Count} rows."),
            Expand = true,
        };

        foreach (var column in worksheet.DataFields.OrderBy(o => o.Position))
        {
            table.AddColumn(column.Name);
        }

        foreach (var row in worksheet.DataSheetRows.OrderBy(o => o.Position))
        {
            table.AddRow(row.DataItems.OrderBy(c => c.Position).Select(c => c.Value).ToArray());
        }

        return table;
    }

    #endregion
}
