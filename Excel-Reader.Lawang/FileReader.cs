using Spire.Doc;
using Spectre.Console;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using FieldType = Microsoft.VisualBasic.FileIO.FieldType;
using Microsoft.VisualBasic.FileIO;

namespace Excel_Reader.Lawang;

public class FileReader
{
    public void ReadPdf(FileInfo fileInfo)
    {
        Console.Clear();
         var rule = new Rule("[aquamarine1 bold]PDF CONTENT[/]").LeftJustified();
        AnsiConsole.Write(rule);
        using var pdf = PdfDocument.Open(fileInfo.FullName);

        foreach(var page in pdf.GetPages())
        {
            AnsiConsole.MarkupLine($"\n[blue bold]Pange Number: {page.Number}[/]\n");
            List<Word> words = page.GetWords().ToList();
            
            if(words.Count() == 0)
            {
                AnsiConsole.MarkupLine("[red]NO WORDS FOUND ON THIS PAGE![]");
                continue;
            }

            //Track the Y-cordinate to detect the change
            double previousY = words[0].BoundingBox.Bottom;
            foreach(var word in words)
            {
                 // Detect if the Y-coordinate has changed significantly (indicating a new line)
                    if (Math.Abs(word.BoundingBox.Bottom - previousY) > 5) // Adjust threshold as needed
                    {
                        Console.WriteLine(); // Print a new line
                    }

                    // Print the word
                    Console.Write(word.Text + " ");
                    
                    // Update the Y-coordinate
                    previousY = word.BoundingBox.Bottom;
            }

            Console.WriteLine();
        }

        AnsiConsole.MarkupLine("[gren bold]PDF READING COMPLTED :notebook:[/]");
    }

    public void ReadCSV(FileInfo fileInfo)
    {
        Console.Clear();
        var rule = new Rule("[aquamarine1 bold]CSV CONTENT[/]").LeftJustified();
        AnsiConsole.Write(rule);

        using TextFieldParser parser = new TextFieldParser(fileInfo.FullName);
        
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;

        List<string[]> valuesByRow = new();
        while(!parser.EndOfData)
        {
            string[]? columns = parser.ReadFields();
            valuesByRow.Add(columns ?? new string[0]);
        }

        View.RenderTable(valuesByRow);

        AnsiConsole.MarkupLine("[green bold]\nCSV READING COMPLETED :notebook:\n[/]");    
    }

    public void ReadDOC(FileInfo fileInfo)
    {
        Console.Clear();
        var rule = new Rule("[aquamarine1 bold]CSV CONTENT[/]").LeftJustified();
        AnsiConsole.Write(rule);

        Document document = new Document();   
        document.LoadFromFile(fileInfo.FullName);
        Console.WriteLine(document.GetText());

        AnsiConsole.MarkupLine("[green bold]\n'.doc' FILE READING COMPLETED :notebook:\n[/]");
            
    }
}
