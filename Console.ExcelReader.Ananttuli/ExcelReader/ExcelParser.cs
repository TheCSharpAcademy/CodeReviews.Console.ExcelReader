using ExcelReader.Models;
using OfficeOpenXml;

namespace ExcelReader;

public static class ExcelParser
{
    public static ParseResult? Parse(string filePath, bool includeHeader)
    {
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(filePath))
            {
                var sheet = package.Workbook.Worksheets[0];

                if (sheet.Dimension.End.Row == 0)
                {
                    return new ParseResult { ErrorMessage = "Empty file cannot be loaded" };
                }

                var cols = ParseCols(sheet, includeHeader);
                var rows = ParseRows(sheet, includeHeader);
                var cells = ParseCells(sheet, rows, cols, includeHeader);

                return new ParseResult { ParsedCols = cols, ParsedRows = rows, ParsedCells = cells };
            }
        }
        catch (Exception ex)
        {
            return new ParseResult { ErrorMessage = $"Error parsing the file. Error: {ex.Message}" };
        }
    }

    private static List<ParsedColResult> ParseCols(ExcelWorksheet sheet, bool includeHeader)
    {
        int numCols = sheet.Dimension.End.Column;

        return Enumerable.Range(0, numCols).Select((_, i) =>
            {
                var colName = includeHeader ? sheet?.Cells[1, i + 1]?.Value?.ToString() ?? ""
                    : "";

                var col = new ParsedColResult
                {
                    Name = colName,
                    Index = i
                };

                return col;
            }
        ).ToList();
    }

    private static List<ParsedRowResult> ParseRows(ExcelWorksheet sheet, bool includeHeader)
    {
        var rows = new List<ParsedRowResult>();

        for (int i = includeHeader ? 1 : 0; i < sheet.Dimension.End.Row; i++)
        {
            rows.Add(new ParsedRowResult { Index = includeHeader ? i - 1 : i });
        }
        return rows;
    }

    private static List<ParsedCellResult> ParseCells(
        ExcelWorksheet sheet,
        List<ParsedRowResult> rows,
        List<ParsedColResult> cols,
        bool includeHeader
    )
    {
        var cells = new List<ParsedCellResult>();

        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < cols.Count; j++)
            {
                cells.Add(
                    new ParsedCellResult
                    {
                        RowIndex = i,
                        ColIndex = j,
                        Content = sheet.Cells?[i + 1 + (includeHeader ? 1 : 0), j + 1]?.Value?.ToString()
                    }
                );
            }
        }

        return cells;
    }
}

public class ParseResult
{
    public List<ParsedColResult> ParsedCols { get; set; } = [];
    public List<ParsedRowResult> ParsedRows { get; set; } = [];
    public List<ParsedCellResult> ParsedCells { get; set; } = [];

    public string? ErrorMessage;
}

public class ParsedColResult
{
    public int Index { get; set; }
    public string? Name { get; set; }
}

public class ParsedRowResult
{
    public int Index { get; set; }
}

public class ParsedCellResult
{
    public int RowIndex { get; set; }
    public int ColIndex { get; set; }

    public string? Content { get; set; } = "";
}
