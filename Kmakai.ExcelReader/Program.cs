using Kmakai.ExcelReader;
using OfficeOpenXml;
using Spectre.Console;

var db = new DataBase();
db.CreateDatabase();

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
var file = new FileInfo(Environment.CurrentDirectory + "/students.xlsx");


using var package = new ExcelPackage(file);

var worksheet = package.Workbook.Worksheets[0];

Console.WriteLine("Reading from excel and Inserting data into database...");
for (int row = 2; row <= worksheet.Dimension.Rows; row++)
{
    var student = new StudentInfo
    {
        Name = worksheet.Cells[row, 1].Value.ToString()!,
        Surname = worksheet.Cells[row, 2].Value.ToString()!,
        Major = worksheet.Cells[row, 3].Value.ToString()!
    };

    db.InsertData(student);
}

Console.WriteLine("Students entered to database!");


var students = db.GetStudents();

var table = new Table();
table.AddColumn("Id");
table.AddColumn("Name");
table.AddColumn("Surname");
table.AddColumn("Major");

foreach (var student in students)
{
    table.AddRow(student.Id.ToString(), student.Name, student.Surname, student.Major);
}

AnsiConsole.Write(table);