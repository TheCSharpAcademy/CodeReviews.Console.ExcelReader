# ExcelReader

## Given Requirements:
- [x] This is an application that reads data from an Excel spreadsheet(and other file formats) into a database.
- [x] When the application starts, it should delete the database if it exists, create a new one, create all tables, read from the file, seed into the database.
- [x] Usage of the EPPlus package and SQL type server.
- [x] Once the database is populated, data will be fetched from it and will be showed in the console.
- [x] Printing messages to the console letting the user know what the app is doing at that moment.
- [x] File templates in the project.

## Features
* Simple UI to manage user file path inputs and(if it's possible) choices to edit the file.
![image](https://github.com/TwilightSaw/CodeReviews.Console.ExcelReader/blob/main/ExcelReader.TwilightSaw/images/ui.png)

* Reading from the next file formats: xlsx, csv, pdf, docx using EPPlus package, iText Kernel, openXML.
![image](https://github.com/TwilightSaw/CodeReviews.Console.ExcelReader/blob/main/ExcelReader.TwilightSaw/images/db_reading.png)

* Creating a database from the file using SQL Server.
> [!IMPORTANT]
> After downloading the project, you should check appsetting.json and write your own data there.
> 
> ![image](https://github.com/TwilightSaw/CodeReviews.Console.ExcelReader/blob/main/ExcelReader.TwilightSaw/images/appsettings.png)

* Writing into the xlsx files a new data, choosing column and adding a new text into the empty cell(edit and delete functionality are not implemented(yet), either you can't choose the exact cell, only the next empty one, so be careful).
![image](https://github.com/TwilightSaw/CodeReviews.Console.ExcelReader/blob/main/ExcelReader.TwilightSaw/images/writing_1.png)
![image](https://github.com/TwilightSaw/CodeReviews.Console.ExcelReader/blob/main/ExcelReader.TwilightSaw/images/writing_2.png)
![image](https://github.com/TwilightSaw/CodeReviews.Console.ExcelReader/blob/main/ExcelReader.TwilightSaw/images/writing_3.png)
![image](https://github.com/TwilightSaw/CodeReviews.Console.ExcelReader/blob/main/ExcelReader.TwilightSaw/images/writing_4.png)

## Challenges and Learned Lessons
- Reading from excel was not so hard, but difficulty growth exponentially when you do it dynamically, then you add other file formats and, eventually, you add writing ability.
- Validating dynamic formats is a really hard task.
- Parsing cells to SQL formats also took some time.

## Areas to Improve
- Explore more posibilities with file formats in .NET.

## Resources Used
- C# Academy guidelines and roadmap.
- ChatGPT for new information as EPPlus, openXML, iText Kernel usage, validation techniques, etc..
- Spectre.Console documentation.
- Various StackOverflow articles.
