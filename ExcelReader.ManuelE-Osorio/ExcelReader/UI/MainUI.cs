using System.Runtime.InteropServices.Marshalling;
using ExcelReader.Models;
using Microsoft.VisualBasic;

namespace ExcelReader.UI;

public class MainUI
{
    public static void WelcomeMessage()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the Excel Reader app!");
        Thread.Sleep(2000);
    }

    public static void InformationMessage(string message)
    {
        Console.WriteLine("Info: " + message);
    }

    public static void ErrorMessage(string message)
    {
        Console.WriteLine("Error: " + message);
    }

    public static void ExitMessage()
    {
        InformationMessage("Exiting app.");
        Console.WriteLine("Thank you for using the Excel Reader app!");
        Thread.Sleep(2000);
    }

    public static void DisplayData(ExcelWorkSheetModel data)
    {
        InformationMessage($"Displaying WorkSheet name: {data.WorkSheetName}, Id: {data.WorkSheetId}");
        foreach(ExcelRowModel row in data.Rows)
        {
            List<object> cellList = [.. row.StringCells, .. row.IntCells, .. row.DoubleCells, .. row.DateCells];

            TableUI.PrintTable(cellList, $"Cell Id: {row.RowId}");
        }
    }
}