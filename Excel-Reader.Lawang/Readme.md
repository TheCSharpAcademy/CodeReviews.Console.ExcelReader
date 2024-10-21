
# Excel-Reader.Lawang
This is an application that reads the data from the excel spreadsheet and seed that data into the SQL database. Data from the Excel is read and written using the EPPlus package. I have also tried reading other types of files such as csv, pdf and doc, which uses streamareader and corresponding libraries, and displays it on the console.
## Features
- can read file with extension .xlsx,.csv, .pdf, .doc and .docx
- data of the file with .xlsx extension is saved inside the database with the name of the table same as the worksheet name.
- The user can choose the file that will be read, by inserting the path.
- data is read from any kind of excel file making it dynamic.
- Can write into the already existing worksheet.
- Can create a new worksheet and write data into it.







 ## Screen shots:

![alt text](<Screenshot from 2024-10-21 19-40-59.png>)

![alt text](<Screenshot from 2024-10-21 19-42-15.png>)

- For testing purposes, testing file is present in the "TestingData" folder you can choose the file by Entering the full path of the file, for example
```
TestingData/data.xlsx
```

![alt text](<Screenshot from 2024-10-21 19-41-17.png>)

- Press 'y' to go to solution of challenge section of the project.

![alt text](<Screenshot from 2024-10-21 19-42-57.png>)

- The data from the csv file is read and displayed in the table format.



## Project Summary
#### What challenges did you face and how did you overcome them?

* I had to install different packages for reading different types of file types, reading data from pdf file and trying to display the text in the console like the text is displayed in the pdf file was difficult. but I achieved that using the Y cordinate position of each word in the pdf file.





## 🛠 Skills Learned
#### EPPlus package
- It was my first time working with EPPlus package, using it for reading and writing data.
- Used Spire.Doc for reading the doc or docx file.
## Feedback

If you have any feedback, please reach out to us at depeshgurung44@gmail.com

