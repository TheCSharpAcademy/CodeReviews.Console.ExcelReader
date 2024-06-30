# Excel Reader

## Project Description

In this application an Excel, CSV or word document (should contain a table)
files are read. [EPPLUS](https://epplussoftware.com/) library is used to
read Excel
and CSV files and [Open-XML-SDK](https://github.com/dotnet/Open-XML-SDK)
library is used to read the tables from word document to process. User can
also export from Excel to CSV and PDF and from CSV to Excel and PDF.  
This Application is Part of Console Application Project
at [CSharpAcademy](https://thecsharpacademy.com/project/15/drinks).

## The Application Requirements

* This is an application that will read data from an Excel spreadsheet
into a database
* When the application starts, it should delete the database if it exists,
create a new one, create all tables, read from Excel, seed into the database.
* You need to use EPPlus package
* You shouldn't read into Json first.
* You can use SQLite or SQL Server (or MySQL if you're using a Mac)
* Once the database is populated, you'll fetch data from it and show it in
the console.
* You don't need any user input
* You should print messages to the console letting the user know what the
app is doing at that moment (i.e. reading from excel; creating tables, etc)
* The application will be written for a known table, you don't need to
make it dynamic.
* When submitting the project for review, you need to include an xls
file that can be read by your application.

## How to run the application

[Microsoft Secrets Manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=linux)
is used to store the email credentials.  
Before running the app please set the Connection String

* dotnet user-secrets set ConnectionString.DefaultConnection
"Server = YourServer; Initial Catalog = ExcelReader; User ID = SA;
Password = YourPassword; TrustServerCertificate=True;"
* 3 samples files in excel, word document and csv format are included.
* User can from the menu select and read the file
