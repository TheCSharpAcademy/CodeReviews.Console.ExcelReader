### Excel Reader

This project is from the hard console section of the C# academy project.

The program can read any excel file you input inside it, to select the path of the file, use the App.config file and change the filePath accordingly.

### Objectives

- This is an application that will read data from an Excel spreadsheet into a database
- When the application starts, it should delete the database if it exists, create a new one, create all tables, read from Excel, seed into the database.
- It uses EPPlus package
- Once the database is populated, it will fetch data from it and show it in the console.
- You don't need any user input
- The application reads to a dynamic table, that way any excel sheet will be able to be read


### Takeaways

At first i used EF for the project but when i tackled the challenge to make the program read from any sheet i realized that the model system of EF was being a hidrance more than helping.
It made me realize that while the EF framework is very useful, there are situations, in this case needing a dynamic table that could be changed on the fly, that the model system is not viable.

So the project was made with pure SQL. Making it easier to generate the table i need for each sheet.