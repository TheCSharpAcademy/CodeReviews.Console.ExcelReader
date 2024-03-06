using System;
using System.IO;
using OfficeOpenXml;
using ExcelReader.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System.Dynamic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ExcelReader.Controllers;

namespace ExcelReader;

public class ExcelReader
{
    public static void Main()
    {
        IHost? app;
        try
        {
            app = StartUp.AppInit();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Thread.Sleep(4000);
            return;
        }
        app.Services.CreateScope()
            .ServiceProvider.GetRequiredService<DataController>().Start();
    }
    public static void Test()
    {
        var filePath = "Template.xlsx";

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        FileInfo existingFile = new(filePath);
        using ExcelPackage package = new(existingFile);
        ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

        var ranges = worksheet.Dimension;
        List<string> properties = [];
        string? cellValue = null;
        List<dynamic> testing = [];
        List<int> dateValues = [];
        var dateRanges = 
            from cell in worksheet.Cells[1, 1, 1, ranges.Columns]
            where cell.Text.ToString().Contains("(Date)")
            select cell;

        var intRanges = 
            from cell in worksheet.Cells[1, 1, 1, ranges.Columns]
            where cell.Text.ToString().Contains("(Int)")
            select cell;
        
        var doubleRanges = 
            from cell in worksheet.Cells[1, 1, 1, ranges.Columns]
            where cell.Text.Contains("(Double)")
            select cell;

        var data = worksheet.Cells[1, 1, ranges.Rows, ranges.Columns]
            .ToDataTable( c =>
        {
            c.AlwaysAllowNull = true;
            c.Mappings.Add(0, worksheet.Cells[1,1].Text, typeof(int));
            
            foreach(ExcelRangeBase cell in intRanges)
            {
                c.Mappings.Add(cell.Start.Column -1 , cell.Value.ToString(), typeof(int));
            }
            
            foreach(ExcelRangeBase cell in doubleRanges)
            {
                c.Mappings.Add(cell.Start.Column -1 , cell.Value.ToString(), typeof(double));
            }

            foreach(ExcelRangeBase cell in dateRanges)
            {
                c.Mappings.Add(cell.Start.Column -1 , cell.Value.ToString(), typeof(DateTime));
            }
        });

        List<ExcelRowModel> listToAdd = [];
        
        for(int i = 0; i < data.Rows.Count; i++)
        {
            listToAdd.Add(new ExcelRowModel
            {
                RowId = (int) data.Rows[i][0],
            });
            for(int j = 1; j < data.Columns.Count; j++)
            {
                switch(data.Rows[i][j])
                // switch(data.Columns[j].ColumnName)
                {
                    case int:
                        listToAdd[^1].IntRows.Add( new ExcelRowDataModel<int>
                            {
                                RowTitle = data.Columns[j].ColumnName,
                                RowValue = (int) data.Rows[i][j]
                            }
                        );
                        break;
                    case double:
                        listToAdd[^1].DoubleRows.Add( new ExcelRowDataModel<double>
                            {
                                RowTitle = data.Columns[j].ColumnName,
                                RowValue = (double) data.Rows[i][j]
                            }
                        );
                        break;
                    case DateTime:
                        listToAdd[^1].DateRows.Add( new ExcelRowDataModel<DateTime>
                            {
                                RowTitle = data.Columns[j].ColumnName,
                                RowValue = (DateTime) data.Rows[i][j]
                            }
                        );
                        break;
                    case string:
                    default:
                        listToAdd[^1].StringRows.Add( new ExcelRowStringModel
                            {
                                RowTitle = data.Columns[j].ColumnName,
                                RowValue = (string) data.Rows[i][j]
                            }
                        );
                        break;
                }
            }
        }

        // foreach (DataRow rows in data.Rows)
        // {
        //     listToAdd.Add(new ExcelRowModel
        //     {
        //         RowId = (int) rows[0],
        //     });
            
        //     foreach (DataColumn column in data.Columns)
        //     {
        //         column.
        //     }
        // }

        // foreach (DataColumn column in data.Columns)
        // {
        //     column.ColumnName
        // }

        // data.Select()

        // foreach (ExcelRowModel row in listToAdd)
        // {
        //     row.IntRows.Add(new ExcelRowDataModel<int>
        //     {
        //         RowTitle = 
        //     });
        // }

        for(int i = 1; i <= ranges.Columns ; i++)
        {
            cellValue = worksheet.Cells[1, i].Value.ToString();
            if(cellValue != null)
                properties.Add(cellValue);
        }

        for(int i = 1; i <= ranges.Rows ; i++)
        {   
            int j= 1;
            var element = new ExpandoObject() as IDictionary<string, object>;
            // dynamic element = new ExcelModel();
            for(j = 1; j <= ranges.Columns ; j++)
            {
                // element.Id = worksheet.Cells[i, j].Value.ToString();
                element.Add(worksheet.Cells[1, j].Value.ToString() ?? "", worksheet.Cells[i,j].GetType());
                // element.SetMember(worksheet.Cells[1, j].Value.ToString() ?? "", worksheet.Cells[i,j].Value);
                
            }
            testing.Add(element);
        }
        var newvar = testing[2];

        var element2 = new ExpandoObject() as IDictionary<string, object>;
        element2.Add("hola", 2);


        Console.WriteLine(testing[2].Date.GetType());
    
    
        var dimElement = new ExpandoObject() as IDictionary<string, object>;
        dimElement.Add("Id", 2);
        dimElement.Add("Name", "Rolando");
        dimElement.Add("Surname", "Perez");
        

        object entity = new();

        foreach (var entry in element2)
        {
            var propertyInfo = entity.GetType().GetProperty(entry.Key);
            if(propertyInfo!=null)
                propertyInfo.SetValue(entity, entry.Value, null);
        }
        Type var2 = element2.GetType();
    }
}