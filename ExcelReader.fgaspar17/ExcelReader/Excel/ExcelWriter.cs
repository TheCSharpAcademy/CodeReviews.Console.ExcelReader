using OfficeOpenXml;
using OfficeOpenXml.Style;
using Spectre.Console;

namespace ExcelReader;

public class ExcelWriter
{
    private Excel _excel;
    // TODO: Ask to the user, pass as parameter, check for invalid paths like the path for the main Excel,
    // PathValidator class
    private string _fileName;
    public ExcelWriter(Excel excel, string fileName)
    {
        _excel = excel;
        _fileName = fileName;
    }

    public bool Write()
    {
        return CreateExcel();
    }

    private bool CreateExcel()
    {
        // Creating an instance 
        // of ExcelPackage 
        ExcelPackage excel = new();

        // Iterate over every WorkSheet
        foreach (var ws in _excel.WorkSheets)
        {
            // name of the sheet 
            var workSheet = excel.Workbook.Worksheets.Add(ws.Name);

            // setting the properties 
            // of the work sheet
            workSheet.DefaultRowHeight = 12;

            // Setting the properties 
            // of the first row 
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;

            // Header of the Excel sheet 
            if (ws.Data != null && ws.Data.Count > 0)
            {
                List<string> columns = ws.Data[0].Keys.ToList();
                for (int i = 0; i < ws.Data[0].Keys.Count; i++)
                {
                    workSheet.Cells[1, i + 1].Value = columns[i];
                    // By default, the column width is not  
                    // set to auto fit for the content 
                    // of the range, so we are using 
                    // AutoFit() method here.  
                    workSheet.Column(i + 1).AutoFit();
                }

                // Inserting the article data into excel 
                // sheet by using the for each loop 
                // As we have values to the first row  
                // we will start with second row 
                int recordIndex = 2;

                foreach (var row in ws.Data)
                {
                    for (int i = 0; i < ws.Data[0].Keys.Count; i++)
                    {
                        workSheet.Cells[recordIndex, i + 1].Value = row[columns[i]];
                    }
                    recordIndex++;
                }

            }
        }

        try
        {
            // file name with .xlsx extension  
            string p_strPath = _fileName;

            if (File.Exists(p_strPath))
            {
                AnsiConsole.MarkupLine("[red]File already exists![/]");
                return false;
            }

            // Create excel file on physical disk  
            FileStream objFileStrm = File.Create(p_strPath);
            objFileStrm.Close();

            // Write content to excel file  
            File.WriteAllBytes(p_strPath, excel.GetAsByteArray());
            //Close Excel package 
            excel.Dispose();
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
            return false;
        }

        return true;
    }
}