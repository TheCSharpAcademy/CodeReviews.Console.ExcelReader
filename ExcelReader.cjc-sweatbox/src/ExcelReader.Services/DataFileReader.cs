using ExcelReader.Models;
using OfficeOpenXml;

namespace ExcelReader.Services;

/// <summary>
/// Reads a generic data file with EPPlus.
/// </summary>
public class DataFileReader : IDataFileReader
{
    #region Methods

    public List<DataSheet> GenerateDataSheets(ExcelPackage package)
    {
        List<DataSheet> dataSheets = [];

        int index = 0;
        foreach (var worksheet in package.Workbook.Worksheets)
        {
            var dataSheet = new DataSheet
            {
                Name = worksheet.Name,
                Position = index++,

                // Columns.
                DataFields = GenerateDataFields(worksheet),

                // Rows and Cells.
                DataSheetRows = GenerateDataRows(worksheet)
            };

            dataSheets.Add(dataSheet);
        }

        return dataSheets;
    }

    public List<DataField> GenerateDataFields(ExcelWorksheet worksheet)
    {
        List<DataField> dataFields = [];

        for (int x = 0; x < worksheet.Dimension.End.Column; x++)
        {
            var dataField = new DataField
            {
                Position = x,
                Name = worksheet.Cells[1, x + 1].Text,
            };

            dataFields.Add(dataField);
        }

        return dataFields;
    }

    public List<DataSheetRow> GenerateDataRows(ExcelWorksheet worksheet)
    {
        List<DataSheetRow> dataRows = [];

        for (int y = 1; y < worksheet.Dimension.End.Row; y++)
        {
            var dataRow = new DataSheetRow
            {
                Position = y - 1
            };

            for (int z = 0; z < worksheet.Dimension.End.Column; z++)
            {
                dataRow.Add(z, worksheet.Cells[y + 1, z + 1].Text);
            }

            // Only add a row if it is not all blank/empty/no data items.
            if (!dataRow.IsBlankRow)
            {
                dataRows.Add(dataRow);
            }
        }

        return dataRows;
    }

    #endregion
}
