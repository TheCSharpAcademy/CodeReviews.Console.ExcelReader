using OfficeOpenXml;
using ExcelReader.Models;
using System.Data;
using ExcelReader.UI;

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

        

        MainUI.InformationMessage($"Accessing the excel worksheet at {existingFile.FullName}");
        ExcelWorksheet worksheet;
        try
        {
            worksheet = package.Workbook.Worksheets.First();
        }
        catch
        {
            throw new Exception("Cannot find the file or it doesn't contains any worksheets.");
        }
        var table = GetDataTableFromExcelWorksheet(worksheet);

        MainUI.InformationMessage("Creating the data model");
        ExcelWorkSheetModel worksheetModel = new()
        {
            WorkSheetName = worksheet.Name,
            Rows = GetRowModelFromDataTable(table)
        };
        MainUI.InformationMessage("Data model creation succesful");
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
            c.EmptyRowStrategy = OfficeOpenXml.Export.ToDataTable.EmptyRowsStrategy.StopAtFirst;
            c.ExcelErrorParsingStrategy = OfficeOpenXml.Export.ToDataTable.ExcelErrorParsingStrategy.HandleExcelErrorsAsBlankCells;
            c.Mappings.Add(IdDefaultColumn, 
                worksheet.Cells[1, IdDefaultColumn + OneBasedOffset].Text, typeof(int));
            
            foreach(ExcelRangeBase cell in intRanges)
            {
                c.Mappings.Add(cell.Start.Column + ZeroBasedOffset, 
                    cell.Value.ToString(), typeof(int));
            }
            
            foreach(ExcelRangeBase cell in doubleRanges)
            {
                c.Mappings.Add(cell.Start.Column + ZeroBasedOffset, 
                    cell.Value.ToString(), typeof(double));
            }

            foreach(ExcelRangeBase cell in dateRanges)
            {
                c.Mappings.Add(cell.Start.Column + ZeroBasedOffset, 
                    cell.Value.ToString(), typeof(DateTime));
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
                var value = data.Rows[i][j];
                if(value is DBNull)
                    value = null;
                    
                switch(data.Columns[j].ColumnName)
                {
                    case var n when n.Contains("(int)", StringComparison.InvariantCultureIgnoreCase):
                        listToAdd[^1].IntCells.Add( new ExcelCellData<int>
                            {
                                CellTitle = data.Columns[j].ColumnName.Replace("(int)", "", 
                                    StringComparison.InvariantCultureIgnoreCase),
                                CellValue = (int?) value 
                            }
                        );
                        break;

                    case var n when n.Contains("(double)", StringComparison.InvariantCultureIgnoreCase):
                        listToAdd[^1].DoubleCells.Add( new ExcelCellData<double>
                            {
                                CellTitle = data.Columns[j].ColumnName.Replace("(double)", "", 
                                    StringComparison.InvariantCultureIgnoreCase),
                                CellValue = (double?) value 
                            }
                        );
                        break;

                    case var n when n.Contains("(date)", StringComparison.InvariantCultureIgnoreCase):
                        listToAdd[^1].DateCells.Add( new ExcelCellData<DateTime>
                            {
                                CellTitle = data.Columns[j].ColumnName.Replace("(date)", "", 
                                    StringComparison.InvariantCultureIgnoreCase),
                                CellValue = (DateTime?) value 
                            }
                        );
                        break;

                    default:
                        listToAdd[^1].StringCells.Add( new ExcelCellString
                            {
                                CellTitle = data.Columns[j].ColumnName,
                                CellValue = (string?) value 
                            }
                        );
                        break;
                }
            }
        }
        return listToAdd;
    }
}