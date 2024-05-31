# Console Excel Reader

A console program to transpose an excel table into an SQL database using a C#
library.  I've decided to import a list of checks.  A function like this could
be used by a user to upload written checks to a database where they could be
compared to the checks coming in to monitor check fraud.


## Requirements

- [x] This is an application that will read data from an Excel spreadsheet
into a database
- [x] When the application starts, it should delete the database if it exists,
create a new one, create all tables, read from Excel, seed into the database.
- [x] You need to use EPPlus package
- [x] shouldn't read into Json first.
- [x] You can use SQLite or SQL Server (or MySQL if you're using a Mac)
- [x] Once the database is populated, you'll fetch data from it and show it
in the console.
- [x] You don't need any user input
- [x] You should print messages to the console letting the user know what the
app is doing at that moment (i.e. reading from excel; creating tables, etc)
- [x] The application will be written for a known table, you don't need to make
it dynamic.
- [x] When submitting the project for review, you need to include an xls file
that can be read by your application.

## Challanges

I started with the excel reader, added in the database, and from there is was 
easy to display the results from the database. I used the status from specre 
console to provide informtion to the user.

## References

<https://www.youtube.com/watch?v=kBwmP-kLEEE>
<https://www.youtube.com/watch?v=DIg_hfexws8>

[EOF]