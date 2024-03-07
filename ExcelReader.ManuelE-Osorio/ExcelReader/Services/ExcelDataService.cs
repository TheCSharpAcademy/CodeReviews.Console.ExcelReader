using OfficeOpenXml;
using ExcelReader.Models;
using System.Data;

namespace ExcelReader.Services;

public class ExcelDataService
{
    private readonly string WorkSheetPath;
    private const int ZeroBasedOffset = -1;
    private const int OneBasedOffset = 1;
    private const int IdDefaultColumn = 0;

    public ExcelDataService(AppVars appVars)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        WorkSheetPath = appVars.Path;
    }

    public ExcelWorkSheetModel GetWorkSheetModel()
    {
        FileInfo existingFile = new(WorkSheetPath);
        using ExcelPackage package = new(existingFile);

        var worksheet = package.Workbook.Worksheets.First();
        var table = GetDataTableFromExcelWorksheet(worksheet);

        ExcelWorkSheetModel worksheetModel = new()
        {
            WorkSheetName = worksheet.Name,
            Rows = GetRowModelFromDataTable(table)
        };
        return worksheetModel;
    }

    private static DataTable GetDataTableFromExcelWorksheet(ExcelWorksheet worksheet)
    {
        var ranges = worksheet.Dimension;

        var dateRanges = 
            from cell in worksheet.Cells[1, 1, 1, ranges.Columns]
            where cell.Text.ToString().Contains("(date)", StringComparison.InvariantCultureIgnoreCase)
            select cell;

        var intRanges = 
            from cell in worksheet.Cells[1, 1, 1, ranges.Columns]
            where cell.Text.ToString().Contains("(int)", StringComparison.InvariantCultureIgnoreCase)
            select cell;

        var doubleRanges = 
            from cell in worksheet.Cells[1, 1, 1, ranges.Columns]
            where cell.Text.Contains("(double)", StringComparison.InvariantCultureIgnoreCase)
            select cell;

        return worksheet.Cells[1, 1, ranges.Rows, ranges.Columns]
            .ToDataTable( c =>
        {
            c.AlwaysAllowNull = true;
            c.Mappings.Add(IdDefaultColumn, 
                worksheet.Cells[1, IdDefaultColumn + OneBasedOffset].Text, typeof(int));
            
            foreach(ExcelRangeBase cell in intRanges)
            {
                c.Mappings.Add(cell.Start.Column + ZeroBasedOffset, 
                    cell.Value.ToString()?.Replace("(Int)", "", 
                    StringComparison.InvariantCultureIgnoreCase), typeof(int));
            }
            
            foreach(ExcelRangeBase cell in doubleRanges)
            {
                c.Mappings.Add(cell.Start.Column + ZeroBasedOffset, 
                    cell.Value.ToString()?.Replace("(Double)", "", 
                    StringComparison.InvariantCultureIgnoreCase), typeof(double));
            }

            foreach(ExcelRangeBase cell in dateRanges)
            {
                c.Mappings.Add(cell.Start.Column + ZeroBasedOffset, 
                    cell.Value.ToString()?.Replace("(Date)", "", 
                    StringComparison.InvariantCultureIgnoreCase), typeof(DateTime));
            }
        });
    }

    private static List<ExcelRowModel> GetRowModelFromDataTable(DataTable data)
    {   
        List<ExcelRowModel> listToAdd = [];
        
        for(int i = 0; i < data.Rows.Count; i++)
        {
            listToAdd.Add(new ExcelRowModel{ RowId = (int) data.Rows[i][0] });

            for(int j = 1; j < data.Columns.Count; j++)
            {
                switch(data.Rows[i][j])
                {
                    case int:
                        listToAdd[^1].IntCells.Add( new ExcelCellData<int>
                            {
                                CellTitle = data.Columns[j].ColumnName,
                                CellValue = (int) data.Rows[i][j]
                            }
                        );
                        break;

                    case double:
                        listToAdd[^1].DoubleCells.Add( new ExcelCellData<double>
                            {
                                CellTitle = data.Columns[j].ColumnName,
                                CellValue = (double) data.Rows[i][j]
                            }
                        );
                        break;

                    case DateTime:
                        listToAdd[^1].DateCells.Add( new ExcelCellData<DateTime>
                            {
                                CellTitle = data.Columns[j].ColumnName,
                                CellValue = (DateTime) data.Rows[i][j]
                            }
                        );
                        break;

                    case string:
                    default:
                        listToAdd[^1].StringCells.Add( new ExcelCellString
                            {
                                CellTitle = data.Columns[j].ColumnName,
                                CellValue = (string) data.Rows[i][j]
                            }
                        );
                        break;
                }
            }
        }
        return listToAdd;
    }
}